using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.CommentVMs;
using LAHGO.Service.ViewModels.DetailVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class DetailService : IDetailService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<AppUser> _userManager;

        public DetailService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;

        }
        public async Task<DetailVM> GetProduct(int id)
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

            Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == id, "ProductColorSizes", "Category", "Photos");
            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(x => !x.IsDeleted);
            List<Color> colors = await _unitOfWork.ColorRepository.GetAllAsync(x => !x.IsDeleted);
            List<Comment> comments = await _unitOfWork.CommentRepository.GetAllAsyncInclude(x => !x.IsDeleted && x.ProductId == id, "User");
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<ProductColorSize> productColorSizes = await _unitOfWork.ProductColorSizeRepository.GetAllAsync(x => !x.IsDeleted);

            DetailVM detailVM = new DetailVM();
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                detailVM = new DetailVM
                {
                    Product = product,
                    CartProducts = basketVMs,
                    Sizes = sizes,
                    Colors = colors,
                    ProductColorSizes = productColorSizes,
                    Comments = comments,
                    User = appUser,
                    Users =users
                };
            }
            else
            {
                detailVM = new DetailVM
                {
                    Product = product,
                    CartProducts = basketVMs,
                    Sizes = sizes,
                    Colors = colors,
                    ProductColorSizes = productColorSizes,
                    Comments = comments,
                    Users = users

                };
            }
           
            return detailVM;


        }
        public async Task PostComent(int? productId, string? userId, IFormCollection collection, CommentVM coment)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {

                if (productId == null)
                    throw new ItemtNoteFoundException($"Item not found");

                if (userId == null)
                {
                    throw new ItemtNoteFoundException($"You need to login first");
                }
                Comment dbreview = new Comment
                {
                    UserId = userId,
                    ProductId = productId,
                    Rating = Convert.ToInt32(collection["rating"]),
                    Date = DateTime.UtcNow.AddHours(+4),
                    Text = coment.Text
                };

                Comment postedreview = await _unitOfWork.CommentRepository.GetAsync(c => c.UserId == userId && c.ProductId == productId);
                if (postedreview != null)
                {
                    _unitOfWork.CommentRepository.Remove(postedreview);
                }
                await _unitOfWork.CommentRepository.AddAsync(dbreview);
                await _unitOfWork.CommitAsync();
                return;
            }
            else
            {
                return;

            }
        }


    }
}
