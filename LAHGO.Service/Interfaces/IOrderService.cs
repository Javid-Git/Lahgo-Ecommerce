using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.OrderVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCreateVM> Index();
        Task CheckOut(Order order);
    }
}
