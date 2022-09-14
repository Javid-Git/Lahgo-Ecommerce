using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.OrderVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    [Authorize(Roles = "User")]
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

        
        public async Task<IActionResult> CheckOut(Order order)
        {

            await _orderService.CheckOut(order);

            return RedirectToAction("index", "home");
        }
    }
}
