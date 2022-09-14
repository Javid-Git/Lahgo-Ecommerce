using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.CartProductVMs;
using LAHGO.Service.ViewModels.CommentVMs;
using LAHGO.Service.ViewModels.DetailVMs;
using LAHGO.Service.ViewModels.ShopVMs;
using LAHGO.Service.ViewModels.SizeVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IUnitOfWork _unitOfWork;


        public ProductController( IUnitOfWork unitOfWork, IDetailService detailService, IBasketService basketService)
        {
            _detailService = detailService;
            _basketService = basketService;
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _unitOfWork.ProductRepository.GetAllAsync(p => p.Name.ToLower().Contains(search.ToLower()));

            return PartialView("_SearchPartial", products);

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
        [HttpPost]
        public async Task<IActionResult> PostComent(int? productId, string? userId, IFormCollection collection, CommentVM coment)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _detailService.PostComent(productId, userId, collection, coment);
                
            }
            else
            {
                return RedirectToAction("login", "account", new { area = "" });

            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Detail");
            }
            else
            {
                return RedirectToAction("Detail", new { id = productId});

            }
        }
    }
}
