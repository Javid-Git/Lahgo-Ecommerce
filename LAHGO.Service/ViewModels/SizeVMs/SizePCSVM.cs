using LAHGO.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.SizeVMs
{
    public class SizePCSVM
    {
        public List<Size> Sizes { get; set; }
        public List<ProductColorSize> ProductColorSizes{ get; set; }
    }
}
