using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.ShopVMs;
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
    public class ShopService : IShopService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<AppUser> _userManager;

        public ShopService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;

        }
        public async Task<ShopVM> GetBasket()
        {
            string basket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            List<CartProductCreateVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basket);
            }
            else
            {
                basketVMs = new List<CartProductCreateVM>();
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
                            CartProductCreateVM basketVM = new CartProductCreateVM
                            {
                                ProductId = item.ProductId,
                                SelectCount = item.Counts
                            };

                            basketVMs.Add(basketVM);
                        }
                    }

                    basket = JsonConvert.SerializeObject(basketVMs);

                    _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);
                }
            }
            foreach (CartProductCreateVM basketVM in basketVMs)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == basketVM.ProductId);

                basketVM.Name = dbproduct.Name;
                basketVM.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                basketVM.Image = dbproduct.MainImage;
            };

            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync(x => !x.IsDeleted);
            List<Photo> photos = await _unitOfWork.PhotoRepository.GetAllAsync(x => !x.IsDeleted);
            List<Category> categories = await _unitOfWork.CategoryRepository.GetAllAsync(x => !x.IsDeleted);
            List<Setting> settings = await _unitOfWork.SettingRepository.GetAllAsync(x => !x.IsDeleted);
            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(x => !x.IsDeleted);
            List<Color> colors = await _unitOfWork.ColorRepository.GetAllAsync(x => !x.IsDeleted);
            List<ProductColorSize> productColorSizes = await _unitOfWork.ProductColorSizeRepository.GetAllAsync(x => !x.IsDeleted);

            ShopVM shopVM = new ShopVM
            {
                Products = products,
                CartProducts = basketVMs,
                Settings = settings,
                Categories = categories,
                ProductColorSizes = productColorSizes,
                Photos = photos,
                Sizes = sizes,
                Colors = colors
            };
            return shopVM;
        }
    }
}
