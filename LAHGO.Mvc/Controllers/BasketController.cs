using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.ShopVMs;
using LAHGO.Service.ViewModels.SizeVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            List<CartProductGetVM> basketVMs = await _basketService.Index();

            return View(await _basketProduct(basketVMs));
        }
        private async Task<List<CartProductGetVM>> _basketProduct(List<CartProductGetVM> basketVMs)
        {

            foreach (CartProductGetVM item in basketVMs)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };
            return basketVMs;
        }
        public async Task<IActionResult> OpenBasket()
        {
            MinicartProductVM basketVMs = await _basketService.OpenBasket();

            foreach (CartProductGetVM item in basketVMs.CartProductGets)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };

            return PartialView("_MinicartPartial", basketVMs);
        }
        
        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            MinicartProductVM basketVMs = await _basketService.DeleteFromBasket(id);

            foreach (CartProductGetVM item in basketVMs.CartProductGets)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };

            return PartialView("_MinicartPartial", basketVMs);

        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            List<CartProductGetVM> basketVMs = await _basketService.DeleteFromCart(id);

            return PartialView("_BasketIndexPartial", await _basketProduct(basketVMs));
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteUpdate()
        {
            List<CartProductCreateVM> basketVMs = await _basketService.DeleteUpdate();

            return Json(basketVMs.Count);
        }
        public async Task<IActionResult> GetSizes(int ColorId, int ProductId)
        {
            SizePCSVM sizePCSVM = await _basketService.GetSizes(ColorId, ProductId);

            
            return PartialView("_SizeContainerPartial", sizePCSVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToBasket(int ProductId, int SizeId, int ColorId)
        {
            List<CartProductCreateVM> basketVMs = await _basketService.AddToCart(ProductId, SizeId, ColorId);


            return Json(basketVMs.Count);
            //return RedirectToAction("Index", "Shop");

        }
        
        public async Task<IActionResult> UpdateCount(int? id, int count)
        {

            MinicartProductVM basketVMs = await _basketService.UpdateCount(id, count);


            foreach (CartProductGetVM item in basketVMs.CartProductGets)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };

            return PartialView("_MinicartPartial", basketVMs);
        }
    }
}
