using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Enums;
using LAHGO.Core.Repositories;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.OrderVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;

        }
        public async Task CheckOut(Order order)
        {

            AppUser appuser = await _userManager.Users.Include(u => u.Orders).Include(u=>u.Baskets).ThenInclude(b=>b.Product).FirstOrDefaultAsync(b=>b.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (Basket basket in appuser.Baskets)
            {
                OrderItem orderItem = new OrderItem
                {
                    Price = basket.Product.DiscountedPrice > 0 ? basket.Product.DiscountedPrice : basket.Product.Price,
                    Count = basket.Counts,
                    ProductId = basket.ProductId,
                    TotalPrice = basket.Product.DiscountedPrice > 0 ? basket.Product.DiscountedPrice * basket.Counts : basket.Product.Price * basket.Counts
                };

                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.AppUserId = appuser.Id;
            order.OrderStatus = OrderStatus.Pending;
            order.TotalPrice = orderItems.Sum(o => o.TotalPrice);

            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                //string basketcookie = HttpContext.Request.Cookies["basket"];
                //List<string> basketVMS = JsonConvert.DeserializeObject<List<string>>(basketcookie);

                //basketVMS.Clear();
                //basketcookie = JsonConvert.SerializeObject(basketVMS);
                //HttpContext.Response.Cookies.Append("basket", basketcookie);

                //string basket = HttpContext.Request.Cookies["basket"];
                //List<BasketVM> basketVMs = null;
                //if (!string.IsNullOrWhiteSpace(basket))
                //{
                //    basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                //}
                //else
                //{
                //    basketVMs = new List<BasketVM>();
                //}

                List<Basket> baskets = await _unitOfWork.BasketRepository.GetAllAsync(b => b.UserId == appuser.Id);
                foreach (Basket dbbasket in baskets)
                {
                    _unitOfWork.BasketRepository.Remove(dbbasket);
                    await _unitOfWork.CommitAsync();
                }

            }
        }

        public async Task<OrderCreateVM>  Index()
        {
            AppUser appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

            List<Basket> baskets = await _unitOfWork.BasketRepository.GetAllAsyncInclude(b => b.UserId == appUser.Id, "Product", "Sizes", "Colors");
            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsyncInclude(b => !b.IsDeleted);
            List<ProductColorSize> productColorSizes = await _unitOfWork.ProductColorSizeRepository.GetAllAsync(b => !b.IsDeleted);
            List<Size> sizes = await _unitOfWork.SizeRepository.GetAllAsync(b => !b.IsDeleted);
            List<Color> colors = await _unitOfWork.ColorRepository.GetAllAsync(b => !b.IsDeleted);

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {

            }

            Order order = new Order
            {
                FullName = appUser.FullName,
                Email = appUser.Email
            };

            OrderCreateVM orderVM = new OrderCreateVM
            {
                Order = order,
                Baskets = baskets,
                ProductColorSizes = productColorSizes,
                Products = products,
                Sizes = sizes,
                Colors = colors
            };

            return orderVM;
        }
    }
}
