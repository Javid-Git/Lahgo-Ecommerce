using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.HomeVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Implementations
{
    public class LayoutService : ILayoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<AppUser> _userManager;

        public LayoutService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;

        }
        public async Task<HomeVM> GetBasket()
        {
            string basket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            List<CartProductGetVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<CartProductGetVM>>(basket);
            }
            else
            {
                basketVMs = new List<CartProductGetVM>();
            }

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    foreach (var item in appUser.Baskets)
                    {
                        if (!basketVMs.Any(b => b.ProductId == item.ProductId))
                        {
                            CartProductGetVM basketVM = new CartProductGetVM
                            {
                                ProductId = item.ProductId,
                                SelectCount = item.Counts,
                                SizeId = item.SizeId,
                                ColorId = item.ColorId
                            };

                            basketVMs.Add(basketVM);
                        }
                    }

                    basket = JsonConvert.SerializeObject(basketVMs);

                    _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);
                }
            }
            foreach (CartProductGetVM basketVM in basketVMs)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == basketVM.ProductId);

                basketVM.Name = dbproduct.Name;
                basketVM.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                basketVM.Image = dbproduct.MainImage;
            };

            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync(x => !x.IsDeleted);
            List<Category> categories = await _unitOfWork.CategoryRepository.GetAllAsync(x => !x.IsDeleted);
            List<Setting> settings = await _unitOfWork.SettingRepository.GetAllAsync(x => !x.IsDeleted);
            List<ProductColorSize> productColorSizes= await _unitOfWork.ProductColorSizeRepository.GetAllAsync(x => !x.IsDeleted);
            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(s => !s.IsDeleted);
            List<Color> colors = await _unitOfWork.ColorRepository.GetAllAsync(s => !s.IsDeleted);

            MinicartProductVM minicartProductVM = new MinicartProductVM
            {
                Sizes = sizes,
                Colors = colors,
                CartProductGets = basketVMs
            };
            HomeVM homeVM = new HomeVM
            {
                Favorites = products.Where(p=>p.IsFavorite).ToList(),
                Products = products,
                CartProducts = basketVMs,
                Settings = settings,
                Categories = categories,
                ProductColorSizes = productColorSizes,
                MinicartProductVM = minicartProductVM
            };
            return homeVM;
        }
    }
}
