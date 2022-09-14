using LAHGO.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.OrderVMs
{
    public class OrderListVM
    {
        public Order Order { get; set; }
        public List<Basket> Baskets { get; set; }
        public bool IsDeleted { get; set; }

    }
}
