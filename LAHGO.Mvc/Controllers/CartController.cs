using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBasketService _basketService;

        public CartController(IBasketService basketService, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IActionResult>  Index()
        {
            MinicartProductVM basketVMs = await _basketService.OpenBasket();
            foreach (CartProductGetVM item in basketVMs.CartProductGets)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };

            return View(basketVMs);
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
        public async Task<IActionResult> UpdateSummary()
        {
            MinicartProductVM basketVMs = await _basketService.OpenBasket();
            foreach (CartProductGetVM item in basketVMs.CartProductGets)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };

            return PartialView("_OrderSummaryPartial", basketVMs);
        }
        public async Task<IActionResult> DeleteFromBasket(int id)
        {
            MinicartProductVM basketVMs = await _basketService.DeleteFromBasket(id);

            foreach (CartProductGetVM item in basketVMs.CartProductGets)
            {
                Product dbproduct = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == item.ProductId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };

            return PartialView("_BasketIndexPartial", basketVMs);
        }

    }
}
