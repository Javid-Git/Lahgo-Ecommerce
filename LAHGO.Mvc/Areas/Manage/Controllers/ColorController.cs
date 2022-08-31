using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.ColorVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        private readonly IMapper _mapper;
        public ColorController(IColorService colorService, IMapper mapper)
        {
            _colorService = colorService;
            _mapper = mapper;
        }
        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<ColorListVM> query = _colorService.GetAllAysnc(status);

            ViewBag.Status = status;
            return View(PageNatedList<ColorListVM>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ColorCreateVM colorCreateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{colorCreateVM.Name}");
                return View();
            }
            await _colorService.CreateAsync(colorCreateVM);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ColorGetVM color = await _colorService.GetById(id);
            return View(color);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ColorUpdateVM colorUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{colorUpdateVM.Name}{colorUpdateVM.Id}");
                return View();
            }
            await _colorService.UpdateAsync(id, colorUpdateVM);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Get(int? status)
        {
            IQueryable<ColorListVM> colorListVMs = _colorService.GetAllAysnc(status);
            return View(colorListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            ColorGetVM colorGetVM = await _colorService.GetById(id);
            return View(colorGetVM);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _colorService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Restore(int id)
        {
            await _colorService.RestoreAsync(id);
            return RedirectToAction("Index");
        }

    }
}
