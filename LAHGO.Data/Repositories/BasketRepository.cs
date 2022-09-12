﻿using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Data.Repositories
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(AppDbContext context) : base(context)
        {

        }
    }
}
