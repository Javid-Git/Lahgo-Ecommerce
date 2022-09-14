using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.SettingVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;
        public SettingController(ISettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<SettingListVM> query = _settingService.GetAllAysnc(status);

            ViewBag.Status = status;
            return View(PageNatedList<SettingListVM>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SettingCreateVM settingCreateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{settingCreateVM.Key}");
                return View();
            }
            await _settingService.CreateAsync(settingCreateVM);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            SettingGetVM setting = await _settingService.GetById(id);
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SettingUpdateVM settingUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{settingUpdateVM.Key}{settingUpdateVM.Value}");
                return View();
            }
            await _settingService.UpdateAsync(id, settingUpdateVM);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Get(int? status)
        {
            IQueryable<SettingListVM> settingListVMs = _settingService.GetAllAysnc(status);
            return View(settingListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            SettingGetVM settingGetVM = await _settingService.GetById(id);
            return View(settingGetVM);
        }
        public async Task<IActionResult> Delete(int id, int page = 1)
        {
            IQueryable<SettingListVM> categoryListVMs = await _settingService.DeleteAsync(id);
            return PartialView("_SettingIndexPartial", PageNatedList<SettingListVM>.Create(page, categoryListVMs, 5));
        }
        public async Task<IActionResult> Restore(int id, int page = 1)
        {
            IQueryable<SettingListVM> categoryListVMs = await _settingService.RestoreAsync(id);
            return PartialView("_SettingIndexPartial", PageNatedList<SettingListVM>.Create(page, categoryListVMs, 5));

        }
    }
}
