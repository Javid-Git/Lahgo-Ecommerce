using LAHGO.Service.ViewModels.CommentVMs;
using LAHGO.Service.ViewModels.ProductVMs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IProductService
    {
        Task CreateAsync(ProductCreateVM productCreateVM);
        IQueryable<ProductListVM> GetAllAysnc(int? status);
        Task<ProductGetVM> GetById(int id);
        Task UpdateAsync(int id, ProductGetVM productGetVM);
        Task DeleteImgAsync(int id);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
