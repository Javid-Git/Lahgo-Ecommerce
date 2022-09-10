using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CartProductVMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.HeaderVMs
{
    public class HeaderVM
    {
        public List<Setting> Settings { get; set; }
        public List<CartProductCreateVM> CartProducts { get; set; }
    }
}
