using LAHGO.Service.ViewModels.CommentVMs;
using LAHGO.Service.ViewModels.DetailVMs;
using LAHGO.Service.ViewModels.ShopVMs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IDetailService
    {
        Task<DetailVM> GetProduct(int id);
        Task PostComent(int? productId, string? userId, IFormCollection collection, CommentVM coment);

    }
}
