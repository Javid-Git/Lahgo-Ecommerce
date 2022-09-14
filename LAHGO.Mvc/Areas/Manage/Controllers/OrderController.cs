using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Enums;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.OrderVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;


        public OrderController(IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(int? status, int page = 1)
        {
            IQueryable<Order> query = await _orderService.GetAllAysnc(status);
           
            ViewBag.Status = status;

            return View(PageNatedList<Order>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            OrderGetVM order = await _orderService.GetUpdate(id);

            if (order == null) return NotFound();
            

            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, OrderStatus OrderStatus, string Comment)
        {
            if (id == null) return BadRequest();


            await _orderService.UpdateAsync(id, OrderStatus, Comment);


            return RedirectToAction("index");
        }

        public async Task<IActionResult> DeleteOrderItem(int id, int orderId)
        {
            List<OrderItem> orderItems = await _orderService.DeleteOrderItem(id, orderId);

            return PartialView("_OrderItemsPartial", orderItems);
        }
    }
}
