using LAHGO.Service.ViewModels.ProductVMs;
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
        Task UpdateAsync(int id, ProductUpdateVM productUpdateVM);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
