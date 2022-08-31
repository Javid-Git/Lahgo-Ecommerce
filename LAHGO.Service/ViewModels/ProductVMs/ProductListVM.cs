using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.ProductVMs
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
        public string Describtion { get; set; }
        public string MainImage { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
    }
}
