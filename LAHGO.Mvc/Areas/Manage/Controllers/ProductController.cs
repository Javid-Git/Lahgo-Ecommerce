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
        private readonly ITypingService _typingService;
        private readonly IMapper _mapper;
        public ProductController(ITypingService typingService, IProductColorSizeService productColorSizeService, ISizeService sizeService, IProductService productService, IMapper mapper, ICategoryService categoryService, IColorService colorService)
        {
            _productService = productService;
            _colorService = colorService;
            _categoryService = categoryService;
            _mapper = mapper;
            _sizeService = sizeService;
            _productColorSizeService = productColorSizeService;
            _typingService = typingService;
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
            ViewBag.Types = _typingService.GetAllAysnc(status);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM, int? status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Types = _typingService.GetAllAysnc(status);
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
            ViewBag.Types = _typingService.GetAllAysnc(status);

            ProductGetVM productUpdateVM = await _productService.GetById(id);

            return View(productUpdateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, ProductGetVM productGetVM, int status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);
            ViewBag.Types = _typingService.GetAllAysnc(status);

            await _productService.UpdateAsync(id, productGetVM);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUnit(int id, int status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);

            PCSGetVM productUpdateVM = await _productColorSizeService.GetById(id);
            return PartialView("_UnitUpdatePartial", productUpdateVM);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateUnit(int id, PCSGetVM productGetVM, int status, int prodId)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);

            PCSGetVM productUpdateVM = await _productColorSizeService.GetById(prodId);

            await _productColorSizeService.UpdateAsync(id ,productGetVM, prodId);

            return PartialView("_UnitUpdatePartial", productUpdateVM);

        }
        [HttpGet]
        public async Task<IActionResult> DeleteImg(int id, int status)
        {
            ViewBag.Categories = _categoryService.GetAllAysnc(status);
            ViewBag.Colors = _colorService.GetAllAysnc(status);
            ViewBag.Sizes = _sizeService.GetAllAysnc(status);
            ViewBag.Types = _typingService.GetAllAysnc(status);

            ProductGetVM productGetVM = await _productService.GetById(id);
            await _productService.DeleteImgAsync(id);

            return PartialView("_DetailImgList", productGetVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Restore(int id)
        {
            await _productService.RestoreAsync(id);
            return RedirectToAction("Index");
        }


    }
}
