using LAHGO.Service.ViewModels.ColorVMs;
using LAHGO.Service.ViewModels.TypingVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface ITypingService
    {
        Task CreateAsync(TypingCreateVM colorPostVM);
        IQueryable<TypingListVM> GetAllAysnc(int? status);
        Task<TypingGetVM> GetById(int id);
        Task UpdateAsync(int id, TypingUpdateVM colorPutVM);
        Task<IQueryable<TypingListVM>> DeleteAsync(int id);
        Task<IQueryable<TypingListVM>> RestoreAsync(int id);
    }
}
