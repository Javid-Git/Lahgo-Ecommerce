using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.ProductVMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CartProductVMs
{
    public class CartProductCreateVM
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int SelectCount { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }

    public class CartProductPostVM
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
    }

    public class MinicartProductVM
    {
        public List<CartProductGetVM> CartProductGets { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Color> Colors { get; set; }
    }
}
