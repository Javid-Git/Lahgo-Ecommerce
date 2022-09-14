using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public string ConfirmationToken { get; set; }
        public string ResetToken { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public List<Basket> Baskets { get; set; }
        public List<Comment> Coments { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public string PasswordResetToken { get; set; }

    }
}
