using AutoMapper;
using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Enums;
using LAHGO.Core.Repositories;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.OrderVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async Task CheckOut(Order order)
        {
            if (order == null)
                throw new ItemtNoteFoundException($"Item not found");

            AppUser appuser = await _userManager.Users.Include(u => u.Orders).Include(u=>u.Baskets).ThenInclude(b=>b.Product).FirstOrDefaultAsync(b=>b.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);

            if (appuser == null)
                throw new ItemtNoteFoundException($"User not found");

            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.AppUserId = appuser.Id;
            order.OrderStatus = OrderStatus.Pending;
            order.FullName = appuser.FullName;

            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.CommitAsync();

            List<OrderItem> orderItems = new List<OrderItem>();

            

            foreach (Basket basket in appuser.Baskets)
            {
                Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == basket.ProductId);
                OrderItem orderItem = new OrderItem
                {
                    Price = basket.Product.DiscountedPrice > 0 ? basket.Product.DiscountedPrice : basket.Product.Price,
                    Count = basket.Counts,
                    ProductId = basket.ProductId,
                    TotalPrice = basket.Product.DiscountedPrice > 0 ? basket.Product.DiscountedPrice * basket.Counts : basket.Product.Price * basket.Counts,
                    OrderId = order.Id,
                    Image = product.MainImage
                };
                orderItems.Add(orderItem);
            }
            await _unitOfWork.OrderItemRepository.AddAllAsync(orderItems);
            await _unitOfWork.CommitAsync();
            order.OrderItems = orderItems;
            order.TotalPrice = orderItems.Sum(o => o.TotalPrice);
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
                string basketcookie = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
                List<CartProductGetVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductGetVM>>(basketcookie);
                foreach (var basket in basketVMs.ToList())
                {
                    basketVMs.Remove(basket);
                    basketcookie = JsonConvert.SerializeObject(basketVMs);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", basketcookie);
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
        public async Task<IQueryable<Order>> GetAllAysnc(int? status)
        {
            
            
            //List<Order> productListVMs = _mapper.Map<List<Order>>(_unitOfWork.OrderRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);

            //IQueryable<Order> query = productListVMs.AsQueryable();

            IQueryable<Order> query = _unitOfWork.OrderRepository.GetAll(r => r.IsDeleted || !r.IsDeleted).Include(o => o.OrderItems);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(c => c.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }
            return query;
        }

        public async Task<OrderGetVM> GetUpdate(int? id)
        {
            OrderGetVM order = _mapper.Map<OrderGetVM>(_unitOfWork.OrderRepository.GetAsync(o => o.Id == id, "OrderItems", "OrderItems.Product").Result);

            return order;
        }
        public async Task UpdateAsync(int? id, OrderStatus orderstatus, string Comment)
        {

            Order order = await _unitOfWork.OrderRepository.GetAsync(o => o.Id == id);

            if (order == null)
                throw new ItemtNoteFoundException($"Item not found");

            if (orderstatus != null)
            {
                order.OrderStatus = orderstatus;

            }
            if (Comment != null)
            {
                order.Comment = Comment;
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<List<OrderItem>> DeleteOrderItem(int id, int orderId)
        {
            OrderItem orderItem = await _unitOfWork.OrderItemRepository.GetAsync(o => o.Id == id);
            if (orderItem == null)
                throw new ItemtNoteFoundException($"Item not found");
            orderItem.IsDeleted = true;
            orderItem.DeletedAt = DateTime.UtcNow.AddHours(4); 

            await _unitOfWork.CommitAsync();

            List<OrderItem> orderItems = await _unitOfWork.OrderItemRepository.GetAllAsyncInclude(o => o.OrderId == orderId, "Product");

            return orderItems;
        }
    }
}
