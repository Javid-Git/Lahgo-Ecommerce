using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.ShopVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }
        public async Task<IActionResult> Index()
        {
            ShopVM shopVM = await _shopService.GetBasket();

            return View(shopVM);
        }
    }
}
