using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.SizeVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IBasketService
    {
        Task<List<CartProductGetVM>> Index();
        Task<List<CartProductCreateVM>> AddToCart(int? ProductId, int SizeId, int ColorId);
        Task<MinicartProductVM> OpenBasket();
        Task<MinicartProductVM> DeleteFromBasket(int? id);
        Task<List<CartProductGetVM>> DeleteFromCart(int? id);
        Task<List<CartProductCreateVM>> DeleteUpdate();
        Task<SizePCSVM> GetSizes(int ColorId, int ProductId);
        Task<MinicartProductVM> UpdateCount(int? id, int count);
    }
}
