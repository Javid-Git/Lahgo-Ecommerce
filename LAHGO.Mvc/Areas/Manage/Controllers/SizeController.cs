using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.SizeVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SizeController : Controller
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public SizeController(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }
        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<SizeListVM> query = _sizeService.GetAllAysnc(status);

            ViewBag.Status = status;
            return View(PageNatedList<SizeListVM>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SizeCreateVM sizeCreateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{sizeCreateVM.Name}");
                return View();
            }
            await _sizeService.CreateAsync(sizeCreateVM);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            SizeGetVM size = await _sizeService.GetById(id);
            return View(size);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SizeUpdateVM sizeUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{sizeUpdateVM.Name}{sizeUpdateVM.Id}");
                return View();
            }
            await _sizeService.UpdateAsync(id, sizeUpdateVM);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Get(int? status)
        {
            IQueryable<SizeListVM> sizeListVMs = _sizeService.GetAllAysnc(status);
            return View(sizeListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            SizeGetVM sizeGetVM = await _sizeService.GetById(id);
            return View(sizeGetVM);
        }
        public async Task<IActionResult> Delete(int id, int page = 1)
        {
            IQueryable<SizeListVM> categoryListVMs = await _sizeService.DeleteAsync(id);
            return PartialView("_SizeIndexPartial", PageNatedList<SizeListVM>.Create(page, categoryListVMs, 5));
        }
        public async Task<IActionResult> Restore(int id, int page = 1)
        {
            IQueryable<SizeListVM> categoryListVMs = await _sizeService.RestoreAsync(id);
            return PartialView("_SizeIndexPartial", PageNatedList<SizeListVM>.Create(page, categoryListVMs, 5));

        }

    }
}
