using AutoMapper;
using LAHGO.Core.Entities;
using LAHGO.Core.Repositories;
using LAHGO.Service.Exceptions;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CategoryVMs;
using System;
using System.Collections.Generic;
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

        public async Task<List<CategoryListVM>> GetAllAysnc()
        {
            List<CategoryListVM> categoryListVMs = _mapper.Map<List<CategoryListVM>>(await _categoryRepository.GetAllAsync(c => !c.IsDeleted));

            return categoryListVMs;
        }

        public async Task<CategoryGetVM> GetById(int id)
        {
            Category category = await _categoryRepository.GetAsync(c => !c.IsDeleted && c.Id == id, "Parent", "Children");

            if (category == null)
                throw new ItemtNoteFoundException($"Item Not Found By Id = {id}");

            CategoryGetVM categoryGetVM = _mapper.Map<CategoryGetVM>(category);

            return categoryGetVM;
        }

        public async Task PostAsync(CategoryPostVM categoryPostVM)
        {
            if (await _categoryRepository.IsExistAsync(c => !c.IsDeleted && c.Name.ToLower() == categoryPostVM.Name.Trim().ToLower()))
                throw new AlreadeExistException($"Category {categoryPostVM.Name} already Exists");

            Category category = _mapper.Map<Category>(categoryPostVM);
            category.Image = "Test.jpg";

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();
        }

        public async Task PutAsync(int id, CategoryPutVM categoryPutVM)
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
