using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.ProductVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper, ICategoryService categoryService, IColorService colorService)
        {
            _productService = productService;
            _colorService = colorService;
            _categoryService = categoryService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create(int? status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            return View();
        }
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            return View();
        }
    }
}
