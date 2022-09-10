using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.HeaderVMs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.ViewComponents
{
    public class ShopHeaderViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopHeaderViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Setting> settings = await _unitOfWork.SettingRepository.GetAllAsync(x => !x.IsDeleted);

            List<CartProductCreateVM> basketVMs = null;
            string basket = HttpContext.Request.Cookies["basket"];
            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basket);

                foreach (CartProductCreateVM basketVM in basketVMs)
                {
                    Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == basketVM.ProductId);

                    basketVM.Name = product.Name;
                    basketVM.Image = product.MainImage;
                    basketVM.Price = product.DiscountedPrice > 0 ? product.DiscountedPrice : product.Price;
                }
            }
            else
            {
                basketVMs = new List<CartProductCreateVM>();
            }
            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            HeaderVM headerVM = new HeaderVM()
            {
                Settings = settings,
                CartProducts = basketVMs
            };

            return View(await Task.FromResult(headerVM));
        }
    }
}
