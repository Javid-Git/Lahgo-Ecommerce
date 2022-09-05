using AutoMapper;
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
        private readonly IProductColorSizeRepository _productColorSizeRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProductColorSizeService(IProductColorSizeRepository productColorSizeRepository, IWebHostEnvironment env, IMapper mapper)
        {
            _productColorSizeRepository = productColorSizeRepository;
            _mapper = mapper;
            _env = env;

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
            ProductColorSize productColorSize = await _productColorSizeRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id, "Product", "Product.Category");

            if (productColorSize == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            PCSGetVM pCSGetVM = _mapper.Map<PCSGetVM>(productColorSize);

            return pCSGetVM;
        }

        public Task RestoreAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, PCSUpdateVM pcsUpdateVM)
        {
            throw new NotImplementedException();
        }
    }
}
