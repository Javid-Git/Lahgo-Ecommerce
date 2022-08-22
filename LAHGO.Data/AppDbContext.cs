using LAHGO.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Color> Colors{ get; set; }
        public DbSet<Size> Sizes{ get; set; }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<ProductColorSize> ProductColorSizes { get; set; }
        public DbSet<Comment> Coments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CommentReply> ComentReplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
