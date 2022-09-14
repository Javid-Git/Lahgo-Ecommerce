using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.DetailVMs;
using LAHGO.Service.ViewModels.ShopVMs;
using LAHGO.Service.ViewModels.SizeVMs;
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
        private readonly IBasketService _basketService;

        public ProductController(IDetailService detailService, IBasketService basketService)
        {
            _detailService = detailService;
            _basketService = basketService;

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
        public async Task<IActionResult> GetSizes(int ColorId, int ProductId)
        {
            SizePCSVM sizePCSVM = await _basketService.GetSizes(ColorId, ProductId);


            return PartialView("_DetailSizePartial", sizePCSVM);
        }
    }
}
