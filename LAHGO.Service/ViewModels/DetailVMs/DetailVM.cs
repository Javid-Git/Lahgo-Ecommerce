using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CartProductVMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.DetailVMs
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public List<Photo> Photos { get; set; }
        public List<Category> Categories { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Comment> Comments{ get; set; }
        public List<Color> Colors { get; set; }
        public List<CartProductCreateVM> CartProducts { get; set; }
        public List<ProductColorSize> ProductColorSizes { get; set; }
        public AppUser User{ get; set; }
        public List<AppUser> Users{ get; set; }
    }
}
