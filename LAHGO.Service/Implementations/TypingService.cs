using AutoMapper;
using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.TypingVMs;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class TypingService : ITypingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public TypingService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        public async Task CreateAsync(TypingCreateVM typingCreateVM)
        {
            Typing typing = _mapper.Map<Typing>(typingCreateVM);
            if (await _unitOfWork.TypingRepository.IsExistAsync(c => c.Name.ToLower() == typingCreateVM.Name.Trim().ToLower()))
            {
                throw new AlreadeExistException($"Type {typingCreateVM.Name} already Exists");
            }
            typing.Name = typingCreateVM.Name;

            await _unitOfWork.TypingRepository.AddAsync(typing);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Typing typing = await _unitOfWork.TypingRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (typing == null)
                throw new ItemtNoteFoundException($"Item not found");

            typing.IsDeleted = true;
            typing.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
        }

        public IQueryable<TypingListVM> GetAllAysnc(int? status)
        {
            List<TypingListVM> typingListVMs = _mapper.Map<List<TypingListVM>>(_unitOfWork.TypingRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);

            IQueryable<TypingListVM> query = typingListVMs.AsQueryable();

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

        public async Task<TypingGetVM> GetById(int id)
        {
            Typing typing = await _unitOfWork.TypingRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id);

            if (typing == null)
                throw new ItemtNoteFoundException($"Item not found");

            TypingGetVM typingGetVM = _mapper.Map<TypingGetVM>(typing);

            return typingGetVM;
        }

        public async Task RestoreAsync(int id)
        {
            Typing typing = await _unitOfWork.TypingRepository.GetAsync(c => c.IsDeleted && c.Id == id);

            if (typing == null)
                throw new ItemtNoteFoundException($"Item not found");

            typing.IsDeleted = false;
            typing.DeletedAt = null;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(int id, TypingUpdateVM typingUpdateVM)
        {
            Typing typing = await _unitOfWork.TypingRepository.GetAsync(c => !c.IsDeleted && c.Id == id || c.IsDeleted);

            if (typing == null)
                throw new ItemtNoteFoundException($"Item not found");


            if (await _unitOfWork.TypingRepository.IsExistAsync(c => c.Name.ToLower() == typingUpdateVM.Name.Trim().ToLower()))
            {
                if (typing.Name == typingUpdateVM.Name)
                {
                    typing.Name = typingUpdateVM.Name;

                    typing.UpdatedAt = DateTime.UtcNow.AddHours(4);

                    await _unitOfWork.CommitAsync();
                }
                else
                {
                    throw new AlreadeExistException($"Type {typingUpdateVM.Name} already Exists");
                }
            }
            typing.Name = typingUpdateVM.Name;


            typing.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
        }
    }
}
