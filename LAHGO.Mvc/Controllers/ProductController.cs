using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.DetailVMs;
using LAHGO.Service.ViewModels.ShopVMs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IDetailService _detailService;
        public ProductController(IDetailService detailService)
        {
            _detailService = detailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int id)
        {
            DetailVM detailVM = await _detailService.GetProduct(id);

            return View(detailVM);
        }
    }
}
