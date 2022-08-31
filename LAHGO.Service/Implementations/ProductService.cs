using AutoMapper;
using LAHGO.Core.Repositories;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.ProductVMs;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;

        public ProductService(IWebHostEnvironment env, IMapper mapper, IProductRepository productRepository )
        {
            _mapper = mapper;
            _env = env;
            _productRepository = productRepository;
        }
        public Task CreateAsync(ProductCreateVM productCreateVM)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ProductListVM> GetAllAysnc(int? status)
        {
            List<ProductListVM> productListVMs = _mapper.Map<List<ProductListVM>>(_productRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);

            IQueryable<ProductListVM> query = productListVMs.AsQueryable();

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

        public Task<ProductGetVM> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, ProductUpdateVM productUpdateVM)
        {
            throw new NotImplementedException();
        }
    }
}
