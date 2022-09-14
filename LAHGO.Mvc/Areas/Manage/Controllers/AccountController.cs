using LAHGO.Core.Entities;
using LAHGO.Service.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == loginVM.Email.Trim().ToUpperInvariant() && u.IsAdmin);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email Or Password Is InCorrect");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", $"Hesabini Bloklanib 10 Deqiqe Sonra Aktive Olacaq Qalan deqiqe {((appUser.LockoutEnd.Value - DateTime.UtcNow).TotalMinutes).ToString("0")}");
                return View(loginVM);
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email Or Password Is InCorrect");
                return View(loginVM);
            }

            return RedirectToAction("index", "home", new { area = "manage" });
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
    
