using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
        public int Count { get; set; }
        public string Describtion { get; set; }
        public string MainImage { get; set; }
        public List<Photo> Photos { get; set; }
        public IEnumerable<Basket> Baskets { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public List<ProductColorSize> ProductColorSizes { get; set; }
        public bool IsNewArrival { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsWashableSilk { get; set; }
        public bool IsLinenShop { get; set; }
        public bool IsFavorite { get; set; }

    }
}
