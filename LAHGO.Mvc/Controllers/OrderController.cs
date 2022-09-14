using LAHGO.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using LAHGO.Core.Repositories;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.OrderVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;


namespace LAHGO.Mvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            OrderCreateVM orderCreateVM = await _orderService.Index();
            return View(orderCreateVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> CheckOut(Order order)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill the form");
                return RedirectToAction("index", "order");
            }
            await _orderService.CheckOut(order);

            return RedirectToAction("index", "home");
        }

        

    }
}
