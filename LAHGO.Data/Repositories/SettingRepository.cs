using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Data.Repositories
{
    public class SettingRepository : Repository<Setting>, ISettingRepository
    {
        public SettingRepository(AppDbContext context) : base(context)
        {

        }
    }
}
