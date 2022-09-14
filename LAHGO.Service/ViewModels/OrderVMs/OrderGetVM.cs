using LAHGO.Core.Entities;
using LAHGO.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LAHGO.Service.ViewModels.OrderVMs
{
    public class OrderGetVM
    {
        public double TotalPrice { get; set; }
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string TownCity { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        public string Comment { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
