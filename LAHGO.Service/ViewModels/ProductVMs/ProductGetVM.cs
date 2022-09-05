using LAHGO.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.ProductVMs
{
    public class ProductGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> DiscountedPrice { get; set; }
        public string Describtion { get; set; }
        public int CategoryId { get; set; }
        public IFormFile MainFormImage { get; set; }
        public List<IFormFile> DetailFormImages { get; set; }
        public List<Color> Colors { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
        public List<Size> Sizes { get; set; }
        public List<int> ColorIds { get; set; }
        public List<int> SizeIds { get; set; }
        public List<int> Counts { get; set; }
        public List<ProductColorSize> ProductColorSizes { get; set; }
    }
}
