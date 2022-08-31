using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Data.Repositories
{
    public class ProductRepository : Repository<Category>, ICategoryRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        }
    }
}
