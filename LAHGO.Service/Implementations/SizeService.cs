using AutoMapper;
using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Interfaces;
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
    public class SizeService : ISizeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public SizeService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        public async Task CreateAsync(SizeCreateVM sizeCreateVM)
        {
            Size size = _mapper.Map<Size>(sizeCreateVM);

            size.Name = sizeCreateVM.Name;

            await _unitOfWork.SizeRepository.AddAsync(size);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Size size = await _unitOfWork.SizeRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (size == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            size.IsDeleted = true;
            size.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
        }

        public IQueryable<SizeListVM> GetAllAysnc(int? status)
        {
            List<SizeListVM> sizeListVMs = _mapper.Map<List<SizeListVM>>(_unitOfWork.SizeRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);

            IQueryable<SizeListVM> query = sizeListVMs.AsQueryable();

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

        public async Task<SizeGetVM> GetById(int id)
        {
            Size size = await _unitOfWork.SizeRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id);

            if (size == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            SizeGetVM sizeGetVM = _mapper.Map<SizeGetVM>(size);

            return sizeGetVM;
        }

        public async Task RestoreAsync(int id)
        {
            Size size = await _unitOfWork.SizeRepository.GetAsync(c => c.IsDeleted && c.Id == id);

            if (size == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            size.IsDeleted = false;
            size.DeletedAt = null;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(int id, SizeUpdateVM sizeUpdateVM)
        {
            Size size = await _unitOfWork.SizeRepository.GetAsync(c => !c.IsDeleted && c.Id == id || c.IsDeleted);

            if (size == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");


            if (await _unitOfWork.SizeRepository.IsExistAsync(c => c.Name.ToLower() == sizeUpdateVM.Name.Trim().ToLower()))
            {
                if (size.Name == sizeUpdateVM.Name)
                {
                    size.Name = sizeUpdateVM.Name;

                    size.UpdatedAt = DateTime.UtcNow.AddHours(4);

                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    throw new AlreadeExistException($"Category {sizeUpdateVM.Name} already Exists");
                }
            }
            size.Name = sizeUpdateVM.Name;


            size.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
        }
    }
}
