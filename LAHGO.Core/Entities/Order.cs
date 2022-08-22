using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class Order : BaseEntity
    {
        public string AppUserId { get; set; }
        public double TotalPrice { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string TownCity { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Comment { get; set; }
        public AppUser AppUser { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
