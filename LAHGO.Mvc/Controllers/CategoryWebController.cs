using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.ShopVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    public class CategoryWebController : Controller
    {
        private readonly IShopService _shopService;
        public CategoryWebController(IShopService shopService)
        {
            _shopService = shopService;
        }
        public async Task<IActionResult> Index(int SizeId, int ColorId, int TypeId, int CategoryId, int count)
        {
            ViewBag.SizeId = SizeId;
            ViewBag.ColorId = ColorId;
            ViewBag.TypeId = TypeId;
            ViewBag.CategoryId = CategoryId;
            ShopVM shopVM = await _shopService.GetBasket(SizeId, ColorId, TypeId, CategoryId, count);

            return View(shopVM);
        }
    }
}
