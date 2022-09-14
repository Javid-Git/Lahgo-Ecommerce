using AutoMapper;
using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.ColorVMs;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public ColorService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public async Task CreateAsync(ColorCreateVM colorCreateVM)
        {

            Color color = _mapper.Map<Color>(colorCreateVM);
            if (await _unitOfWork.ColorRepository.IsExistAsync(c => c.Name.ToLower() == colorCreateVM.Name.Trim().ToLower()))
            {
                throw new AlreadeExistException($"Color {colorCreateVM.Name} already Exists");
            }
            color.Name = colorCreateVM.Name;
            color.Code = colorCreateVM.Code;

            await _unitOfWork.ColorRepository.AddAsync(color);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IQueryable<ColorListVM>> DeleteAsync(int id)
        {
            Color color = await _unitOfWork.ColorRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (color == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            color.IsDeleted = true;
            color.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
            List<ColorListVM> colorListVMs = _mapper.Map<List<ColorListVM>>(_unitOfWork.ColorRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);
            IQueryable<ColorListVM> query = colorListVMs.AsQueryable();

            return query;
        }

        public IQueryable<ColorListVM> GetAllAysnc(int? status)
        {
            List<ColorListVM> colorListVMs = _mapper.Map<List<ColorListVM>>(_unitOfWork.ColorRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);
            if (colorListVMs == null)
                throw new ItemtNoteFoundException($"Items not found");

            IQueryable<ColorListVM> query = colorListVMs.AsQueryable();

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

        public async Task<ColorGetVM> GetById(int id)
        {
            Color color = await _unitOfWork.ColorRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id);

            if (color == null)
                throw new ItemtNoteFoundException($"Items not found");

            ColorGetVM colorGetVM = _mapper.Map<ColorGetVM>(color);

            return colorGetVM;
        }

        public async Task<IQueryable<ColorListVM>> RestoreAsync(int id)
        {
            Color color = await _unitOfWork.ColorRepository.GetAsync(c => c.IsDeleted && c.Id == id);

            if (color == null)
                throw new ItemtNoteFoundException($"Items not found");

            color.IsDeleted = false;
            color.DeletedAt = null;

            await _unitOfWork.CommitAsync();
            List<ColorListVM> colorListVMs = _mapper.Map<List<ColorListVM>>(_unitOfWork.ColorRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);
            IQueryable<ColorListVM> query = colorListVMs.AsQueryable();

            return query;
        }

        public async Task UpdateAsync(int id, ColorUpdateVM colorUpdateVM)
        {
            Color color = await _unitOfWork.ColorRepository.GetAsync(c => !c.IsDeleted && c.Id == id || c.IsDeleted);

            if (color == null)
                throw new ItemtNoteFoundException($"Items not found");


            if (await _unitOfWork.ColorRepository.IsExistAsync(c => c.Name.ToLower() == colorUpdateVM.Name.Trim().ToLower()))
            {
                if (color.Name == colorUpdateVM.Name)
                {
                    color.Name = colorUpdateVM.Name;

                    color.UpdatedAt = DateTime.UtcNow.AddHours(4);

                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    throw new AlreadeExistException($"Color {colorUpdateVM.Name} already Exists");
                }
            }
            if (await _unitOfWork.ColorRepository.IsExistAsync(c => c.Code.ToLower() == colorUpdateVM.Name.Trim().ToLower()))
            {
                if (color.Code == colorUpdateVM.Code)
                {
                    color.Code = colorUpdateVM.Code;

                    color.UpdatedAt = DateTime.UtcNow.AddHours(4);

                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    throw new AlreadeExistException($"Color {colorUpdateVM.Code} already Exists");
                }
            }

            color.Name = colorUpdateVM.Name;
            color.Code = colorUpdateVM.Code;
            color.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
        }
    }
}
