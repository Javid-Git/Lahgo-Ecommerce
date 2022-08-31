using LAHGO.Service.ViewModels.CategoryVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryCreateVM categoryPostVM);
        IQueryable<CategoryListVM> GetAllAysnc(int? status);
        Task<CategoryGetVM> GetById(int id);
        Task UpdateAsync(int id, CategoryUpdateVM categoryPutVM);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
