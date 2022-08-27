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
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM categoryPostVM)
        {
            await _categoryService.CreateAsync(categoryPostVM);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
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

        [HttpPut]
        public async Task<IActionResult> Update(int id, CategoryUpdateVM categoryUpdateVm)
        {
            await _categoryService.UpdateAsync(id, categoryUpdateVm);
            return View();
        }
    }
}
