using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public string Image { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
