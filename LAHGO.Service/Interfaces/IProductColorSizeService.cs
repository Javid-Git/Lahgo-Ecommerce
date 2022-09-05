using LAHGO.Service.ViewModels.PCSVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IProductColorSizeService
    {
        Task CreateAsync(PCSCreateVM pcsCreateVM);
        IQueryable<PCSListVM> GetAllAysnc(int? status);
        Task<PCSGetVM> GetById(int id);
        Task UpdateAsync(int id, PCSUpdateVM pcsUpdateVM);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
