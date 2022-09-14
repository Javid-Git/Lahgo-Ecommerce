﻿using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CartProductVMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.HomeVMs
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Product> Favorites { get; set; }
        public List<Setting> Settings { get; set; }
        public List<Category> Categories { get; set; }
        public MinicartProductVM MinicartProductVM { get; set; }
        public List<CartProductGetVM> CartProducts { get; set; }
        public List<ProductColorSize> ProductColorSizes { get; set; }
    }
}