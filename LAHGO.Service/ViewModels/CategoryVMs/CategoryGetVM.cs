using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.CategoryVMs
{
    public class CategoryGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile FormImage { get; set; }
        public string Image { get; set; }

    }
}
