using AutoMapper;
using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.SettingVMs;
using LAHGO.Service.ViewModels.SizeVMs;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public SettingService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        public async Task CreateAsync(SettingCreateVM settingCreateVM)
        {
            Setting setting = _mapper.Map<Setting>(settingCreateVM);
            if (await _unitOfWork.SettingRepository.IsExistAsync(c => c.Key.ToLower() == settingCreateVM.Key.Trim().ToLower()))
            {
                throw new AlreadeExistException($"Setting key : {settingCreateVM.Key} already Exists");
            }
            setting.Key = settingCreateVM.Key;
            setting.Value = settingCreateVM.Value;

            await _unitOfWork.SettingRepository.AddAsync(setting);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IQueryable<SettingListVM>> DeleteAsync(int id)
        {
            Setting setting = await _unitOfWork.SettingRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (setting == null)
                throw new ItemtNoteFoundException($"Item not found");

            setting.IsDeleted = true;
            setting.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
            List<SettingListVM> colorListVMs = _mapper.Map<List<SettingListVM>>(_unitOfWork.SettingRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);
            IQueryable<SettingListVM> query = colorListVMs.AsQueryable();

            return query;
        }

        public IQueryable<SettingListVM> GetAllAysnc(int? status)
        {
            List<SettingListVM> settingListVMs = _mapper.Map<List<SettingListVM>>(_unitOfWork.SettingRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);

            IQueryable<SettingListVM> query = settingListVMs.AsQueryable();

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(c => c.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }
            return query;
        }

        public async Task<SettingGetVM> GetById(int id)
        {
            Setting setting = await _unitOfWork.SettingRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id);

            if (setting == null)
                throw new ItemtNoteFoundException($"Item not found");

            SettingGetVM settingGetVM = _mapper.Map<SettingGetVM>(setting);

            return settingGetVM;
        }

        public async Task<IQueryable<SettingListVM>> RestoreAsync(int id)
        {
            Setting setting = await _unitOfWork.SettingRepository.GetAsync(c => c.IsDeleted && c.Id == id);

            if (setting == null)
                throw new ItemtNoteFoundException($"Item not found");

            setting.IsDeleted = false;
            setting.DeletedAt = null;

            await _unitOfWork.CommitAsync();
            List<SettingListVM> colorListVMs = _mapper.Map<List<SettingListVM>>(_unitOfWork.SettingRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);
            IQueryable<SettingListVM> query = colorListVMs.AsQueryable();

            return query;
        }

        public async Task UpdateAsync(int id, SettingUpdateVM settingUpdateVM)
        {
            Setting setting = await _unitOfWork.SettingRepository.GetAsync(c => !c.IsDeleted && c.Id == id || c.IsDeleted);

            if (setting == null)
                throw new ItemtNoteFoundException($"Item not found");


            if (await _unitOfWork.SettingRepository.IsExistAsync(c => c.Key.ToLower() == settingUpdateVM.Key.Trim().ToLower()))
            {
                if (setting.Key == settingUpdateVM.Key)
                {
                    setting.Key = settingUpdateVM.Key;

                    setting.UpdatedAt = DateTime.UtcNow.AddHours(4);

                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    throw new AlreadeExistException($"Setting {settingUpdateVM.Key} already Exists");
                }
            }
            setting.Key = settingUpdateVM.Key;
            setting.Value = settingUpdateVM.Value;

            setting.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
        }
    }
}
