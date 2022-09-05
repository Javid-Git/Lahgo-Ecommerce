using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.PCSVMs
{
    public class PCSCreateVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int Count { get; set; }
    }
}
