using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
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
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public BasketService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<CartProductCreateVM>> AddToCart(int? id, int? count)
        {
            if (id == null)
            {
                throw new Exception("Exception while fetching.");
            }
            Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Exception while fetching.");
            }
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

            if (basketVMs.Exists(b => b.ProductId == id))
            {
                basketVMs.Find(b => b.ProductId == id).SelectCount++;
            }
            else
            {
                if (count != null)
                {
                    CartProductCreateVM basketVM = new CartProductCreateVM
                    {
                        ProductId = product.Id,
                        SelectCount = (int)count
                    };
                    basketVMs.Add(basketVM);

                }
                else
                {
                    CartProductCreateVM basketVM = new CartProductCreateVM
                    {
                        ProductId = product.Id,
                        SelectCount = 1
                    };
                    basketVMs.Add(basketVM);

                }


            }
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                    if (dbBasketproduct != null)
                    {
                        dbBasketproduct.Counts += 1;
                    }
                    else
                    {
                        Basket newBasket = new Basket
                        {
                            ProductId = (int)id,
                            UserId = appUser.Id,
                            Counts = 1
                        };

                        appUser.Baskets.Add(newBasket);
                    }
                }
                else
                {
                    List<Basket> baskets = new List<Basket>
                    {
                        new Basket{ProductId = (int)id, Counts = 1}
                    };
                    appUser.Baskets = baskets;
                }
                await _unitOfWork.CommitAsync();
            }
            basket = JsonConvert.SerializeObject(basketVMs);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);

            return basketVMs;
        }

        public Task DeleteFromBasket(int? id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFromCart(int? id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUpdate()
        {
            throw new NotImplementedException();
        }

        public Task OpenBasket()
        {
            throw new NotImplementedException();
        }
    }
}
