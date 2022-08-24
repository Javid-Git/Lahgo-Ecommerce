using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Count { get; set; }
        public string Describtion { get; set; }
        public string MainImage { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public IEnumerable<Basket> Baskets { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public IEnumerable<ProductColorSize> ProductColorSizes { get; set; }
        public bool IsNewArrivel { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsWashableSilk { get; set; }
        public bool IsLinenShop { get; set; }

    }
}
