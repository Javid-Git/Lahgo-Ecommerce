using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.SizeVMs;
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
        public async Task<List<CartProductGetVM>> Index()
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

            return basketVMs;
        }
        public async Task<List<CartProductCreateVM>> AddToCart(int? ProductId, int SizeId, int ColorId)
        {
            if (ProductId == null)
            {
                throw new Exception("Exception while fetching.");
            }
            Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == ProductId);
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
            
            //foreach (var singlebasket in basketVMs)
            //{
            //    if (singlebasket.ProductId == ProductId && singlebasket.ColorId == ColorId && singlebasket.SizeId == SizeId)
            //    {
            //        singlebasket.SelectCount++;
            //    }
            //}
            
            if (basketVMs.Exists(b => b.ProductId == ProductId &&  b.ColorId == ColorId && b.SizeId == SizeId))
            {
                basketVMs.Find(b => b.ProductId == ProductId && b.ColorId == ColorId && b.SizeId == SizeId).SelectCount++;
            }
            else
            {
                CartProductCreateVM basketVM = new CartProductCreateVM
                {
                    ProductId = product.Id,
                    SizeId = SizeId,
                    ColorId = ColorId,
                    SelectCount = 1
                };
                basketVMs.Add(basketVM);
                //if (count != null)
                //{
                //    CartProductCreateVM basketVM = new CartProductCreateVM
                //    {
                //        ProductId = product.Id,
                //        SelectCount = (int)count
                //    };
                //    basketVMs.Add(basketVM);

                //}
                //else
                //{
                //    CartProductCreateVM basketVM = new CartProductCreateVM
                //    {
                //        ProductId = product.Id,
                //        SelectCount = 1
                //    };
                //    basketVMs.Add(basketVM);
                //}
            }
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == ProductId);
                    if (dbBasketproduct != null)
                    {
                        dbBasketproduct.Counts += 1;
                    }
                    else
                    {
                        Basket newBasket = new Basket
                        {
                            ProductId = (int)ProductId,
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
                        new Basket{ProductId = (int)ProductId, Counts = 1}
                    };
                    appUser.Baskets = baskets;
                }
                await _unitOfWork.CommitAsync();
            }
            basket = JsonConvert.SerializeObject(basketVMs);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);

            return basketVMs;
        }

        public async Task<MinicartProductVM> DeleteFromBasket(int? id)
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
            List<CartProductGetVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductGetVM>>(basket);

            if (string.IsNullOrWhiteSpace(basket)) throw new Exception("Exception while fetching.");

            CartProductGetVM basketVM = basketVMs.Find(b => b.ProductId == id);
            if (basket == null) throw new Exception("Exception while fetching.");

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
                List<Basket> userbasket = await _unitOfWork.BasketRepository.GetAllAsync(b => b.UserId == appUser.Id);

                if (userbasket != null && userbasket.Count() > 0)
                {
                    Basket dbBasketproduct = userbasket.FirstOrDefault(p => p.ProductId == id);
                    if (dbBasketproduct != null)
                    {
                        userbasket.Remove(dbBasketproduct);
                        _unitOfWork.BasketRepository.Remove(dbBasketproduct);
                        await _unitOfWork.CommitAsync();
                    }
                    else
                    {
                         throw new Exception("Exception while fetching.");

                    }
                }
                else
                {
                     throw new Exception("Exception while fetching.");
                }
            }
            basketVMs.Remove(basketVM);
            await _unitOfWork.CommitAsync();
            basket = JsonConvert.SerializeObject(basketVMs);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);

            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(s => !s.IsDeleted);
            List<Color> colors = await _unitOfWork.ColorRepository.GetAllAsync(s => !s.IsDeleted);

            MinicartProductVM minicartProductVM = new MinicartProductVM
            {
                CartProductGets = basketVMs,
                Sizes = sizes,
                Colors = colors

            };
            return minicartProductVM;
        }

        public async Task<List<CartProductGetVM>> DeleteFromCart(int? id)
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
            List<CartProductGetVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductGetVM>>(basket);

            if (string.IsNullOrWhiteSpace(basket)) throw new Exception("Exception while fetching.");

            CartProductGetVM basketVM = basketVMs.Find(b => b.ProductId == id);
            if (basket == null) throw new Exception("Exception while fetching.");
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                    if (dbBasketproduct != null)
                    {
                        appUser.Baskets.Remove(dbBasketproduct);
                        _unitOfWork.BasketRepository.Remove(dbBasketproduct);
                        await _unitOfWork.CommitAsync();
                        //_context.Baskets.Remove(dbBasketproduct);
                    }
                    else
                    {
                        throw new Exception("Exception while fetching.");

                    }
                }
                else
                {
                    throw new Exception("Exception while fetching.");
                }
            }
            basketVMs.Remove(basketVM);

            basket = JsonConvert.SerializeObject(basketVMs);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);

            return basketVMs;
        }

        public async Task<List<CartProductCreateVM>> DeleteUpdate()
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

            return basketVMs;

        }
        public async Task<MinicartProductVM> OpenBasket()
        {
            string basket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            List<CartProductGetVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductGetVM>>(basket); ;
            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(s => !s.IsDeleted);
            List<Color> colors = await _unitOfWork.ColorRepository.GetAllAsync(s => !s.IsDeleted);

            basket = JsonConvert.SerializeObject(basketVMs);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);

            MinicartProductVM minicartProductVM = new MinicartProductVM
            {
                CartProductGets = basketVMs,
                Sizes = sizes,
                Colors = colors

            };
            return minicartProductVM;
        }
        public async Task<SizePCSVM> GetSizes(int ColorId, int ProductId)
        {

            List<ProductColorSize> productColorSizes = await _unitOfWork.ProductColorSizeRepository.GetAllAsync(p => p.ColorId == ColorId && p.ProductId == ProductId);
            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(s => !s.IsDeleted);
            SizePCSVM sizePCSVM = new SizePCSVM
            {
                ProductColorSizes = productColorSizes,
                Sizes = sizes
            };
            return sizePCSVM;
        }

        public async Task<MinicartProductVM> UpdateCount(int? id, int count)
        {
            if (id == null) throw new Exception("Exception while fetching.");

            Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Exception while fetching.");
            }

            string basket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

            List<CartProductGetVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<CartProductGetVM>>(basket);

                CartProductGetVM basketVM = basketVMs.FirstOrDefault(b => b.ProductId == id);

                if (basketVM == null) throw new Exception("Exception while fetching.");
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name && !u.IsAdmin);

                    if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                    {
                        Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                        if (dbBasketproduct != null)
                        {
                            dbBasketproduct.Counts = count <= 0 ? 1 : count;
                            await _unitOfWork.CommitAsync();
                        }
                        else
                        {
                            throw new Exception("Exception while fetching.");
                        }
                    }
                }

                basketVM.SelectCount = count <= 0 ? 1 : count;

                basket = JsonConvert.SerializeObject(basketVMs);

                _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basket);
            }
            else
            {
                throw new Exception("Exception while fetching.");
            }

            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(s => !s.IsDeleted);
            List<Color> colors = await _unitOfWork.ColorRepository.GetAllAsync(s => !s.IsDeleted);

            MinicartProductVM minicartProductVM = new MinicartProductVM
            {
                CartProductGets = basketVMs,
                Sizes = sizes,
                Colors = colors

            };
            return minicartProductVM;
        }
    }
}
