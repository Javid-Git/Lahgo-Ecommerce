using LAHGO.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CartProductVMs
{
    public class CartProductCreateVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int SelectCount { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}
