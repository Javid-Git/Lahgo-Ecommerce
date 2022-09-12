using LAHGO.Service.ViewModels.ShopVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IShopService
    {
        Task<ShopVM> GetBasket();
    }
}
