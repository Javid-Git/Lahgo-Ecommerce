using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CategoryVMs;
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
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
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

        public IQueryable<CategoryListVM> GetAllAysnc(int? status)
        {
            List<CategoryListVM> categoryListVMs = _mapper.Map<List<CategoryListVM>>( _categoryRepository.GetAllAsync(c => !c.IsDeleted).Result);

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
            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id, "Parent", "Children");

            if (category == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            CategoryGetVM categoryGetVM = _mapper.Map<CategoryGetVM>(category);

            return categoryGetVM;
        }

        public async Task CreateAsync(CategoryCreateVM categoryCreateVM)
        {
            if (await _categoryRepository.IsExistAsync(c => !c.IsDeleted && c.Name.ToLower() == categoryCreateVM.Name.Trim().ToLower()))
                throw new AlreadeExistException($"Category {categoryCreateVM.Name} already Exists");

            Category category = _mapper.Map<Category>(categoryCreateVM);

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();
        }

        public async Task UpdateAsync(int id, CategoryUpdateVM categoryPutVM)
        {
            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id);

            if (category == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            if (await _categoryRepository.IsExistAsync(c => !c.IsDeleted && c.Name.ToLower() == categoryPutVM.Name.Trim().ToLower()))
                throw new AlreadeExistException($"Category {categoryPutVM.Name} already Exists");

            category.Name = categoryPutVM.Name;
            category.Image = "category-1.jpg";
            category.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _categoryRepository.CommitAsync();
        }
    }
}
