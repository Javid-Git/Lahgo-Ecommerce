using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.HomeVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface ILayoutService
    {
        Task<HomeVM> GetBasket();
        //Task<IDictionary<string, string>> GetSetting();
    }
}
