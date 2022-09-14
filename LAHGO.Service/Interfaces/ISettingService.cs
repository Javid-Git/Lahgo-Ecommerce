using LAHGO.Service.ViewModels.SettingVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface ISettingService
    {
        Task CreateAsync(SettingCreateVM settingCreateVM);
        IQueryable<SettingListVM> GetAllAysnc(int? status);
        Task<SettingGetVM> GetById(int id);
        Task UpdateAsync(int id, SettingUpdateVM settingUpdateVM);
        Task<IQueryable<SettingListVM>> DeleteAsync(int id);
        Task<IQueryable<SettingListVM>> RestoreAsync(int id);
    }
}
