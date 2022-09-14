using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.CategoryVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<CategoryListVM> query = _categoryService.GetAllAysnc(status);  
            
            ViewBag.Status = status;
            return View(PageNatedList<CategoryListVM>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM categoryCreateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{categoryCreateVM.Name}{categoryCreateVM.FormImage}");
                return View();  
            }
            await _categoryService.CreateAsync(categoryCreateVM);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            CategoryGetVM category = await _categoryService.GetById(id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryUpdateVM categoryUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{categoryUpdateVM.Name}{categoryUpdateVM.Id}{categoryUpdateVM.FormImage}");
                return View();
            }
            await _categoryService.UpdateAsync(id, categoryUpdateVM);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Get(int? status)
        {
            IQueryable<CategoryListVM> categoryListVMs =  _categoryService.GetAllAysnc(status);
            return View(categoryListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            CategoryGetVM categoryGetVM = await _categoryService.GetById(id);
            return View(categoryGetVM);
        }
        public async Task<IActionResult> Delete(int id, int page = 1)
        {
            IQueryable<CategoryListVM> categoryListVMs =  await _categoryService.DeleteAsync(id);
            return PartialView("_CategoryIndexPartial", PageNatedList<CategoryListVM>.Create(page, categoryListVMs, 5));
        }
        public async Task<IActionResult> Restore(int id, int page = 1)
        {
            IQueryable<CategoryListVM> categoryListVMs = await _categoryService.RestoreAsync(id);
            return PartialView("_CategoryIndexPartial", PageNatedList<CategoryListVM>.Create(page, categoryListVMs, 5));

        }

    }
}
