using LAHGO.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.PCSVMs
{
    public class PCSGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> DiscountedPrice { get; set; }
        public string Describtion { get; set; }
        public int CategoryId { get; set; }
        public IFormFile MainFormImage { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
        public List<IFormFile> DetailFormImages { get; set; }
        public List<Color> Colors { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int SizeId { get; set; }
        public Size Size { get; set; }
        public List<Size> Sizes { get; set; }

    }
}
