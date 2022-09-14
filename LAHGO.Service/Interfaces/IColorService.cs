using LAHGO.Service.ViewModels.ColorVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IColorService
    {
        Task CreateAsync(ColorCreateVM colorPostVM);
        IQueryable<ColorListVM> GetAllAysnc(int? status);
        Task<ColorGetVM> GetById(int id);
        Task UpdateAsync(int id, ColorUpdateVM colorPutVM);
        Task<IQueryable<ColorListVM>> DeleteAsync(int id);
        Task<IQueryable<ColorListVM>> RestoreAsync(int id);
    }
}
