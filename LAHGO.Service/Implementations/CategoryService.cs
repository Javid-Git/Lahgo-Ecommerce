using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Extensions;
using LAHGO.Service.Helpers;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CategoryVMs;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static LAHGO.Service.Exceptions.ItemNotFoundException;

namespace LAHGO.Service.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _env = env;
        }

        public async Task DeleteAsync(int id)
        {
            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (category == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _categoryRepository.CommitAsync();
        }
        public async Task RestoreAsync(int id)
        {
            Category category = await _categoryRepository.GetAsync(c => c.IsDeleted && c.Id == id);

            if (category == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            category.IsDeleted = false;
            category.DeletedAt = null;

            await _categoryRepository.CommitAsync();
        }

        public IQueryable<CategoryListVM> GetAllAysnc(int? status)
        {
            List<CategoryListVM> categoryListVMs = _mapper.Map<List<CategoryListVM>>(_categoryRepository.GetAllAsync(r => r.IsDeleted || !r.IsDeleted).Result);

            IQueryable<CategoryListVM> query = categoryListVMs.AsQueryable();

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

        public async Task<CategoryGetVM> GetById(int id)
        {
            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted || c.IsDeleted && c.Id == id);

            if (category == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            CategoryGetVM categoryGetVM = _mapper.Map<CategoryGetVM>(category);

            return categoryGetVM;
        }

        public async Task CreateAsync(CategoryCreateVM categoryCreateVM)
        {
            
            Category category = _mapper.Map<Category>(categoryCreateVM);

            category.Name = categoryCreateVM.Name;
            category.Image = await categoryCreateVM.FormImage.CreateAsync(_env, "Manage", "assets", "img", "categories");

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();
        }

        public async Task UpdateAsync(int id, CategoryUpdateVM categoryUpdateVM)
        {
            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id || c.IsDeleted);

            if (category == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            
            if (await _categoryRepository.IsExistAsync(c => c.Name.ToLower() == categoryUpdateVM.Name.Trim().ToLower()))
            {
                if (category.Name == categoryUpdateVM.Name)
                {
                    category.Name = categoryUpdateVM.Name;
                    if (categoryUpdateVM.FormImage != null)
                    {
                        FileHelper.DeleteFile(_env, category.Image, "Manage", "assets", "img", "categories");
                        category.Image = await categoryUpdateVM.FormImage.CreateAsync(_env, "Manage", "assets", "img", "categories");
                    }

                    category.UpdatedAt = DateTime.UtcNow.AddHours(4);

                    await _categoryRepository.CommitAsync();
                }
                else
                {
                    throw new AlreadeExistException($"Category {categoryUpdateVM.Name} already Exists");
                }
            }
            category.Name = categoryUpdateVM.Name;
            if (categoryUpdateVM.FormImage != null)
            {
                FileHelper.DeleteFile(_env, category.Image, "Manage", "assets", "img", "categories");
                category.Image = await categoryUpdateVM.FormImage.CreateAsync(_env, "Manage", "assets", "img", "categories");
            }

            category.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _categoryRepository.CommitAsync();
        }
    }
}
