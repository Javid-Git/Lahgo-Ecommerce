using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class Color : BaseEntity  
    {
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }


    }
}
