using LAHGO.Service.ViewModels.CartProductVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IBasketService
    {
        Task<List<CartProductCreateVM>> AddToCart(int? id, int? count);
        Task DeleteFromBasket(int? id);
        Task DeleteFromCart(int? id);
        Task OpenBasket();
        Task DeleteUpdate();
    }
}
