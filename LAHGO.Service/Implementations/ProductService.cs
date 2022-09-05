using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Extensions;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.PCSVMs;
using LAHGO.Service.ViewModels.ProductVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductColorSizeRepository _productColorSizeRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IWebHostEnvironment _env;

        public ProductService(IProductColorSizeRepository productColorSizeRepository, IPhotoRepository photoRepository, IWebHostEnvironment env, IMapper mapper, IProductRepository productRepository )
        {
            _mapper = mapper;
            _env = env;
            _productRepository = productRepository;
            _photoRepository = photoRepository;
            _productColorSizeRepository = productColorSizeRepository;
        }
        public async Task CreateAsync(ProductCreateVM productCreateVM)
        {
            Product product = _mapper.Map<Product>(productCreateVM);
            product.MainImage = await productCreateVM.MainFormImage.CreateAsync(_env, "Manage", "assets", "img", "productMainImg");
            
            product.Count = productCreateVM.Counts.Sum();

            await _productRepository.AddAsync(product);
            await _productRepository.CommitAsync();


            List<ProductColorSize> productColorSizes = new List<ProductColorSize>();

            for (int i = 0; i < productCreateVM.ColorIds.Count(); i++)
            {
                ProductColorSize productColorSize = new ProductColorSize
                {
                    ColorId = productCreateVM.ColorIds[i],
                    SizeId = productCreateVM.SizeIds[i],
                    Count = productCreateVM.Counts[i],
                    ProductId = product.Id
                };
                await _productColorSizeRepository.AddAsync(productColorSize);
                await _productColorSizeRepository.CommitAsync();
            }


            if (productCreateVM.DetailFormImages != null)
            {
                foreach (IFormFile image in productCreateVM.DetailFormImages)
                {
                    Photo photo = new Photo();
                    photo.Image = await FileManager.CreateAsync(image, _env, "Manage", "assets", "img", "productMainImg");
                    photo.ProductId = product.Id;
                    await _photoRepository.AddAsync(photo);
                    await _photoRepository.CommitAsync();
                }
            }
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

        public async Task<ProductGetVM> GetById(int id)
        {
            Product product = await _productRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id, "ProductColorSizes", "ProductColorSizes.Color", "ProductColorSizes.Size");

            if (product == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            ProductGetVM productGetVM = _mapper.Map<ProductGetVM>(product);

            return productGetVM;
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
