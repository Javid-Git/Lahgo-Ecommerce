using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    public class BasketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            string basket = HttpContext.Request.Cookies["basket"];
            List<CartProductCreateVM> basketVMs = null;
            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basket);
            }
            else
            {
                basketVMs = new List<CartProductCreateVM>();
            }
            return View(await _basketProduct(basketVMs));
        }
        private async Task<List<CartProductCreateVM>> _basketProduct(List<CartProductCreateVM> basketVMs)
        {

            foreach (CartProductCreateVM item in basketVMs)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };
            return basketVMs;
        }
        public async Task<IActionResult> AddToBasket(int? id, int? count)
        {
            List<CartProductCreateVM> basketVMs = await _basketService.AddToCart(id, count);
            


            return Json(basketVMs.Count);
        }
        public async Task<IActionResult> OpenBasket()
        {


            string basket = HttpContext.Request.Cookies["basket"];
            List<CartProductCreateVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basket); ;



            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);


            //return Json(basketVMs.Count);
            return PartialView("_AddToCartPartial", await _basketProduct(basketVMs));
        }
        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            string basket = HttpContext.Request.Cookies["basket"];
            List<CartProductCreateVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basket);

            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();

            CartProductCreateVM basketVM = basketVMs.Find(b => b.ProductId == id);
            if (basket == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                    if (dbBasketproduct != null)
                    {
                        appUser.Baskets.Remove(dbBasketproduct);
                        _unitOfWork.BasketRepository.Remove(dbBasketproduct);
                        await _unitOfWork.CommitAsync();
                    }
                    else
                    {
                        return NotFound();

                    }
                }
                else
                {
                    return NotFound();
                }
            }
            basketVMs.Remove(basketVM);
            await _unitOfWork.CommitAsync();
            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_AddToCartPartial", await _basketProduct(basketVMs));

        }
        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            string basket = HttpContext.Request.Cookies["basket"];
            List<CartProductCreateVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basket);

            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();

            CartProductCreateVM basketVM = basketVMs.Find(b => b.ProductId == id);
            if (basket == null) return NotFound();
            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

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
                        return NotFound();

                    }
                }
                else
                {
                    return NotFound();
                }
            }
            basketVMs.Remove(basketVM);

            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketIndexPartial", await _basketProduct(basketVMs));
        }
        public async Task<IActionResult> DeleteUpdate()
        {
            string basket = Request.Cookies["basket"];
            List<CartProductCreateVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basket);
            }
            else
            {
                basketVMs = new List<CartProductCreateVM>();
            }

            return Json(basketVMs.Count);

        }


    }
}
