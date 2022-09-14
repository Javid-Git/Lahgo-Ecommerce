using AutoMapper;
using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Extensions;
using LAHGO.Service.Helpers;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CommentVMs;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IWebHostEnvironment _env;

        public ProductService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, IWebHostEnvironment env, IMapper mapper )
        {
            _mapper = mapper;
            _env = env;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task CreateAsync(ProductCreateVM productCreateVM)
        {
            Product product = _mapper.Map<Product>(productCreateVM);
            product.MainImage = await productCreateVM.MainFormImage.CreateAsync(_env, "Manage", "assets", "img", "productMainImg");
            product.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.Count = productCreateVM.Counts.Sum();

            if (await _unitOfWork.ProductRepository.IsExistAsync(c => c.Name.ToLower() == productCreateVM.Name.Trim().ToLower()))
            {
                throw new AlreadeExistException($"Product {productCreateVM.Name} already Exists");

            }
            if (productCreateVM.IsFavorite)
            {
                product.IsFavorite = true;
            }
            if (productCreateVM.IsBestSeller)
            {
                product.IsBestSeller = true;
            }
            if (productCreateVM.IsLinenShop)
            {
                product.IsLinenShop = true;
            }
            if (productCreateVM.IsNewArrival)
            {
                product.IsNewArrival = true;
            }
            if (productCreateVM.IsWashableSilk)
            {
                product.IsWashableSilk = true;
            }
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CommitAsync();


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
                await _unitOfWork.ProductColorSizeRepository.AddAsync(productColorSize);
                await _unitOfWork.CommitAsync();
            }
            for (int i = 0; i < productCreateVM.TypeIds.Count(); i++)
            {
                ProductTyping productTyping = new ProductTyping
                {
                    TypingId = productCreateVM.TypeIds[i],
                    ProductId = product.Id
                };
                await _unitOfWork.ProductTypingRepository.AddAsync(productTyping);
                await _unitOfWork.CommitAsync();
            }

            if (productCreateVM.DetailFormImages != null)
            {
                foreach (IFormFile image in productCreateVM.DetailFormImages)
                {
                    Photo photo = new Photo();
                    photo.Image = await FileManager.CreateAsync(image, _env, "Manage", "assets", "img", "productDetailImages");
                    photo.ProductId = product.Id;
                    await _unitOfWork.PhotoRepository.AddAsync(photo);
                    await _unitOfWork.CommitAsync();
                }
            }
            
        }

        public async Task DeleteAsync(int id)
        {
            Product product = await _unitOfWork.ProductRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (product == null)
                throw new ItemtNoteFoundException($"Items not found");

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();
        }

        public IQueryable<ProductListVM> GetAllAysnc(int? status)
        {
            List<ProductListVM> productListVMs = _mapper.Map<List<ProductListVM>>(_unitOfWork.ProductRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);

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

            Product product = await _unitOfWork.ProductRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == id, "ProductColorSizes", "ProductColorSizes.Color", "ProductColorSizes.Size", "Photos", "Typings");

            if (product == null)
            {
                Photo photo = await _unitOfWork.PhotoRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

                Product productnew = await _unitOfWork.ProductRepository.GetAsync(c => (!c.IsDeleted || c.IsDeleted) && c.Id == photo.ProductId, "ProductColorSizes", "ProductColorSizes.Color", "ProductColorSizes.Size", "Photos", "Typings");
               
                if (productnew == null)
                {
                    throw new ItemtNoteFoundException($"Items not found");
                }

                ProductGetVM newproductGetVM = _mapper.Map<ProductGetVM>(productnew);

                return newproductGetVM;
            }

            ProductGetVM productGetVM = _mapper.Map<ProductGetVM>(product);

            return productGetVM;
        }

        public async Task RestoreAsync(int id)
        {
            Product product = await _unitOfWork.ProductRepository.GetAsync(c => c.IsDeleted && c.Id == id);

            if (product == null)
                throw new ItemtNoteFoundException($"Items not found");

            product.IsDeleted = false;
            product.DeletedAt = null;

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(int id, ProductGetVM productGetVM)
        {
            Product product = await _unitOfWork.ProductRepository.GetAsync(p => p.Id == id);

            if (product == null)
                throw new ItemtNoteFoundException($"Items not found");

            if (productGetVM.ColorIds != null)
            {
                for (int i = 0; i < productGetVM.ColorIds.Count(); i++)
                {
                    ProductColorSize productColorSize = new ProductColorSize
                    {
                        ColorId = productGetVM.ColorIds[i],
                        SizeId = productGetVM.SizeIds[i],
                        Count = productGetVM.Counts[i],
                        ProductId = product.Id
                    };
                    await _unitOfWork.ProductColorSizeRepository.AddAsync(productColorSize);
                    await _unitOfWork.CommitAsync();
                }
            }

            if (productGetVM.TypeIds != null)
            {
                List<ProductTyping> productTypings = await _unitOfWork.ProductTypingRepository.GetAllAsync(pt => pt.ProductId == id);
                _unitOfWork.ProductTypingRepository.RemoveAllAsync(productTypings);
            }
            if (productGetVM.TypeIds != null)
            {
                for (int i = 0; i < productGetVM.TypeIds.Count(); i++)
                {
                    ProductTyping productTyping = new ProductTyping
                    {
                        TypingId = productGetVM.TypeIds[i],
                        ProductId = product.Id
                    };
                    await _unitOfWork.ProductTypingRepository.AddAsync(productTyping);
                    await _unitOfWork.CommitAsync();
                }
            }
            

            if (productGetVM.MainFormImage != null)
            {
                FileHelper.DeleteFile(_env, product.MainImage, "Manage", "assets", "img", "productMainImg");
                product.MainImage = await productGetVM.MainFormImage.CreateAsync(_env, "Manage", "assets", "img", "productMainImg");

            };

            if (productGetVM.DetailFormImages != null)
            {
                List<Photo> photos = await _unitOfWork.PhotoRepository.GetAllAsync(p => p.ProductId == product.Id);
                foreach (Photo photo in photos)
                {
                    FileHelper.DeleteFile(_env, photo.Image, "Manage", "assets", "img", "productDetailImages");
                    _unitOfWork.PhotoRepository.Remove(photo);
                    await _unitOfWork.CommitAsync();
                }
                foreach (IFormFile image in productGetVM.DetailFormImages)
                {
                    Photo photo = new Photo();
                    photo.Image = await FileManager.CreateAsync(image, _env, "Manage", "assets", "img", "productDetailImages"); ;
                    photo.ProductId = product.Id;
                    await _unitOfWork.PhotoRepository.AddAsync(photo);
                    await _unitOfWork.CommitAsync();
                }
            }
            if (productGetVM.Price != null)
            {
                product.Price = (double)productGetVM.Price;
            }
            if (productGetVM.DiscountedPrice != null)
            {
                product.DiscountedPrice = (double)productGetVM.DiscountedPrice;
            }
            if (productGetVM.Describtion != null)
            {
                product.Describtion = productGetVM.Describtion;
            }

            if (productGetVM.Name != null)
            {
                if (await _unitOfWork.ProductRepository.IsExistAsync(c => c.Name.ToLower() == productGetVM.Name.Trim().ToLower()))
                {
                    if (product.Name == productGetVM.Name)
                    {
                        product.Name = productGetVM.Name;
                        product.UpdatedAt = DateTime.UtcNow.AddHours(4);

                        if (productGetVM.IsFavorite != false)
                        {
                            product.IsFavorite = true;
                        }
                        else
                        {
                            product.IsFavorite = false;
                        }
                        if (productGetVM.IsBestSeller != false)
                        {
                            product.IsBestSeller = true;
                        }
                        else
                        {
                            product.IsBestSeller = false;
                        }
                        if (productGetVM.IsLinenShop != false)
                        {
                            product.IsLinenShop = true;
                        }
                        else
                        {
                            product.IsLinenShop = false;
                        }
                        if (productGetVM.IsNewArrival != false)
                        {
                            product.IsNewArrival = true;
                        }
                        else
                        {
                            product.IsNewArrival = false;
                        }
                        if (productGetVM.IsWashableSilk != false)
                        {
                            product.IsWashableSilk = true;
                        }
                        else
                        {
                            product.IsWashableSilk = false;
                        }
                        await _unitOfWork.CommitAsync();
                    }
                    else
                    {

                        await _unitOfWork.CommitAsync();
                        throw new AlreadeExistException($"Product {productGetVM.Name} already Exists");
                    }
                }
                else
                {
                    product.Name = productGetVM.Name;
                    product.UpdatedAt = DateTime.UtcNow.AddHours(4);

                    if (productGetVM.IsFavorite != false)
                    {
                        product.IsFavorite = true;
                    }
                    else
                    {
                        product.IsFavorite = false;
                    }
                    if (productGetVM.IsBestSeller != false)
                    {
                        product.IsBestSeller = true;
                    }
                    else
                    {
                        product.IsBestSeller = false;
                    }
                    if (productGetVM.IsLinenShop != false)
                    {
                        product.IsLinenShop = true;
                    }
                    else
                    {
                        product.IsLinenShop = false;
                    }
                    if (productGetVM.IsNewArrival != false)
                    {
                        product.IsNewArrival = true;
                    }
                    else
                    {
                        product.IsNewArrival = false;
                    }
                    if (productGetVM.IsWashableSilk != false)
                    {
                        product.IsWashableSilk = true;
                    }
                    else
                    {
                        product.IsWashableSilk = false;
                    }
                    await _unitOfWork.CommitAsync();
                }

            }
        }

        public async Task DeleteImgAsync(int id)
        {
            Photo photo = await _unitOfWork.PhotoRepository.GetAsync(p => p.Id == id);

            if (photo == null)
                throw new ItemtNoteFoundException($"Items not found");

            photo.IsDeleted = true;
            photo.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _unitOfWork.CommitAsync();

        }


    }
}
