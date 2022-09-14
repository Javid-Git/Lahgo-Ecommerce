using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class ProductTyping : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int TypingId { get; set; }
        public Typing Typing { get; set; }
    }
}
