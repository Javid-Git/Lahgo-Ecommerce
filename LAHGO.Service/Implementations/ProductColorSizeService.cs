using AutoMapper;
using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.PCSVMs;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class ProductColorSizeService : IProductColorSizeService
    {
        private readonly IUnitOfWork _unitOfWork;


        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProductColorSizeService(IUnitOfWork unitOfWork, IWebHostEnvironment env, IMapper mapper)
        {
            _mapper = mapper;
            _env = env;
            _unitOfWork = unitOfWork;

        }
        public Task CreateAsync(PCSCreateVM pcsCreateVM)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PCSListVM> GetAllAysnc(int? status)
        {
            throw new NotImplementedException();
        }

        public async Task<PCSGetVM> GetById(int id)
        {
            ProductColorSize productColorSize = await _unitOfWork.ProductColorSizeRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id, "Product", "Product.Category");

            if (productColorSize == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            PCSGetVM pCSGetVM = _mapper.Map<PCSGetVM>(productColorSize);

            return pCSGetVM;
        }

        public Task RestoreAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, PCSGetVM pcsGetVM, int prodId)
        {
            ProductColorSize productColorSize = await _unitOfWork.ProductColorSizeRepository.GetAsync(p => (!p.IsDeleted || p.IsDeleted) && p.Id == pcsGetVM.Id, "Product");
            Product product = await _unitOfWork.ProductRepository.GetAsync(c => c.Id == prodId);

            if (productColorSize == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {pcsGetVM.Id}");

            productColorSize.ColorId = pcsGetVM.ColorId;
            productColorSize.SizeId = pcsGetVM.SizeId;
            productColorSize.Count = pcsGetVM.Count;
            product.CategoryId = pcsGetVM.CategoryId;

            await _unitOfWork.CommitAsync();
            await _unitOfWork.CommitAsync();
        }
    }
}
