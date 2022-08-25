using LAHGO.Service.ViewModels.CategoryVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface ICategoryService
    {
        Task PostAsync(CategoryPostVM categoryPostVM);
        Task<List<CategoryListVM>> GetAllAysnc();
        Task<CategoryGetVM> GetById(int id);
        Task PutAsync(int id, CategoryPutVM categoryPutVM);
        Task DeleteAsync(int id);
    }
}
