using LAHGO.Service.ViewModels.SizeVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface ISizeService
    {
        Task CreateAsync(SizeCreateVM sizeCreateVM);
        IQueryable<SizeListVM> GetAllAysnc(int? status);
        Task<SizeGetVM> GetById(int id);
        Task UpdateAsync(int id, SizeUpdateVM sizeUpdateVM);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
