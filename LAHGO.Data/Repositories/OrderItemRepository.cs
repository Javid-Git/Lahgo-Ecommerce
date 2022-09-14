using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Data.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext context) : base(context)
        {

        }
    }
}
