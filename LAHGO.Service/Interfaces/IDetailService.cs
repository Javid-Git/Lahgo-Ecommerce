using LAHGO.Service.ViewModels.DetailVMs;
using LAHGO.Service.ViewModels.ShopVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IDetailService
    {
        Task<DetailVM> GetProduct(int id);
    }
}
