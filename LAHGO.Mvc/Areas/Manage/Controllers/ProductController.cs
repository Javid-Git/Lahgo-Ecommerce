using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.PCSVMs;
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
        private readonly IProductColorSizeService _productColorSizeService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public ProductController(IProductColorSizeService productColorSizeService, ISizeService sizeService, IProductService productService, IMapper mapper, ICategoryService categoryService, IColorService colorService)
        {
            _productService = productService;
            _colorService = colorService;
            _categoryService = categoryService;
            _mapper = mapper;
            _sizeService = sizeService;
            _productColorSizeService = productColorSizeService;
        }
        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<ProductListVM> query = _productService.GetAllAysnc(status);
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Status = status;
            return View(PageNatedList<ProductListVM>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Create(int? status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM, int? status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{productCreateVM.Name}{productCreateVM.MainFormImage}{productCreateVM.DetailFormImages}{productCreateVM.Price}{productCreateVM.DiscountedPrice}");
                return View();
            }
            await _productService.CreateAsync(productCreateVM);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetProductColorSizePartial(int? status)
        {
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);

            return PartialView("_ProductColorSizePatial");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id, int status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);

            ProductGetVM productUpdateVM = await _productService.GetById(id);

            return View(productUpdateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductGetVM productGetVM, int status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);


            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUnit(int id)
        {
            PCSGetVM productUpdateVM = await _productColorSizeService.GetById(id);
            return PartialView("_UnitUpdatePartial", productUpdateVM);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateUnit(int id, ProductGetVM productGetVM)
        {

           
        }
    }
}
