using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.ProductVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public ProductController(ISizeService sizeService, IProductService productService, IMapper mapper, ICategoryService categoryService, IColorService colorService)
        {
            _productService = productService;
            _colorService = colorService;
            _categoryService = categoryService;
            _mapper = mapper;
            _sizeService = sizeService;
        }
        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<ProductListVM> query = _productService.GetAllAysnc(status);

            ViewBag.Status = status;
            return View(PageNatedList<ProductListVM>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Create(int? status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _colorService.GetAllAysnc(status);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            return View();
        }

        public async Task<IActionResult> GetProductColorSizePartial(int? status)
        {
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _colorService.GetAllAysnc(status);

            return PartialView("_ProductColorSizePatial");
        }
    }
}
