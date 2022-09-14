using AutoMapper;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels;
using LAHGO.Service.ViewModels.TypingVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TypingController : Controller
    {
        private readonly ITypingService _typingService;
        private readonly IMapper _mapper;
        public TypingController(ITypingService typingService, IMapper mapper)
        {
            _typingService = typingService;
            _mapper = mapper;
        }
        public IActionResult Index(int? status, int page = 1)
        {
            IQueryable<TypingListVM> query = _typingService.GetAllAysnc(status);

            ViewBag.Status = status;
            return View(PageNatedList<TypingListVM>.Create(page, query, 5));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TypingCreateVM typingCreateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{typingCreateVM.Name}");
                return View();
            }
            await _typingService.CreateAsync(typingCreateVM);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            TypingGetVM typing = await _typingService.GetById(id);
            return View(typing);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, TypingUpdateVM typingUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{typingUpdateVM.Name}{typingUpdateVM.Id}");
                return View();
            }
            await _typingService.UpdateAsync(id, typingUpdateVM);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Get(int? status)
        {
            IQueryable<TypingListVM> typingListVMs = _typingService.GetAllAysnc(status);
            return View(typingListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            TypingGetVM typingGetVM = await _typingService.GetById(id);
            return View(typingGetVM);
        }
        public async Task<IActionResult> Delete(int id, int page = 1)
        {
            IQueryable<TypingListVM> categoryListVMs = await _typingService.DeleteAsync(id);
            return PartialView("_TypingIndexPartial", PageNatedList<TypingListVM>.Create(page, categoryListVMs, 5));
        }
        public async Task<IActionResult> Restore(int id, int page = 1)
        {
            IQueryable<TypingListVM> categoryListVMs = await _typingService.RestoreAsync(id);
            return PartialView("_TypingIndexPartial", PageNatedList<TypingListVM>.Create(page, categoryListVMs, 5));

        }

    }
}
