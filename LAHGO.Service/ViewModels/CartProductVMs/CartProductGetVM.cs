using LAHGO.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CartProductVMs
{
    
    public class CartProductGetVM
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int SelectCount { get; set; }
        public List<Size> Sizes{ get; set; }
        public List<Color> Colors { get; set; }

    }
}
