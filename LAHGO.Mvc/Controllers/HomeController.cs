using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.HomeVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILayoutService _layoutService;
        public HomeController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = await _layoutService.GetBasket();

            return View(homeVM);
        }
    }
}
