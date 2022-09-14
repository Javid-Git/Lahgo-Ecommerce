using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Data.Repositories
{
    public class ProductTypingRepository : Repository<ProductTyping>, IProductTypingRepository
    {
        public ProductTypingRepository(AppDbContext context) : base(context)
        {

        }
    }
}
