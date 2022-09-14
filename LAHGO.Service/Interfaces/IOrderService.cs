using LAHGO.Core.Entities;
using LAHGO.Core.Enums;
using LAHGO.Service.ViewModels.OrderVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCreateVM> Index();
        Task CheckOut(Order order);
        Task<IQueryable<Order>> GetAllAysnc(int? status);
        Task<OrderGetVM> GetUpdate(int? id);
        Task UpdateAsync(int? id, OrderStatus orderstatus, string Comment);
        Task<List<OrderItem>> DeleteOrderItem(int id, int orderId);
    }
}
