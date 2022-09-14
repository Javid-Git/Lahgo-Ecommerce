using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class Typing : BaseEntity
    {
        public string Name { get; set; }
        public Product Product { get; set; }
    }
}
