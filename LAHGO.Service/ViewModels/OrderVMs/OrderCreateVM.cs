using LAHGO.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.OrderVMs
{
    public class OrderCreateVM
    {
        public Order Order { get; set; }
        public List<Basket> Baskets { get; set; }
        public List<ProductColorSize> ProductColorSizes { get; set; }
        public List<Product> Products { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }
    }
}
