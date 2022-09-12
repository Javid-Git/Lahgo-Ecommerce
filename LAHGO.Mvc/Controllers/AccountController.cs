using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.AccountVMs;
using LAHGO.Service.ViewModels.CartProductVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accountService = accountService;
        }
        #region Roles
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "User" });

        //    return Content("Success!");
        //}
        #endregion
        #region SuperAdmin
        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    AppUser appuser = new AppUser
        //    {
        //        FullName = "Super Admin",
        //        UserName = "SuperAdmin",
        //        Email = "SuperAdmin@gmail.com",
        //        IsAdmin = true

        //    };
        //    await _userManager.CreateAsync(appuser, "JJadmin-2000");
        //    await _userManager.AddToRoleAsync(appuser, "SuperAdmin");
        //    return Content("Super Admin: Success!");
        //}
        #endregion
        [HttpGet]
        public IActionResult Register()
        {
            RegisterVM registerVM= new RegisterVM();

           
            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", $"{registerVM.FullName}{registerVM.Email}{registerVM.Password}{registerVM.ConfirmPasword}");
                return View();
            }
            AppUser appUser = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };
            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            result = await _userManager.AddToRoleAsync(appUser, "User");
            return RedirectToAction("index", "home", new { area = "" });

        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginVM loginVM = new LoginVM();

            return View(loginVM);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email or password is incorrect");

                return View();
            }
            AppUser appuser = await _userManager.FindByEmailAsync(login.Email);
            if (appuser == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(login);
            }
            if (appuser.IsAdmin)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(login);

            }
            if (!await _userManager.CheckPasswordAsync(appuser, login.Password))
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(login);
            }
            await _signInManager.SignInAsync(appuser, (bool)login.RememberMe);
            //string basketCookie = HttpContext.Request.Cookies["basket"];
            //if (!string.IsNullOrWhiteSpace(basketCookie))
            //{
            //    List<CartProductCreateVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basketCookie);
            //    List<Basket> baskets = new List<Basket>();
            //    foreach (CartProductCreateVM basketVM in basketVMs)
            //    {
            //        if (appuser.Baskets != null && appuser.Baskets.Count() > 0)
            //        {
            //            Basket dbBasketproduct = appuser.Baskets.FirstOrDefault(b => b.ProductId != basketVM.ProductId);

            //            if (dbBasketproduct == null)
            //            {
            //                Basket basket = new Basket
            //                {
            //                    UserId = appuser.Id,
            //                    ProductId = basketVM.ProductId,
            //                    Counts = basketVM.SelectCount
            //                };

            //                baskets.Add(basket);
            //            }
            //            else
            //            {
            //                //exsitedBasket.Count = basketVM.Count;
            //                dbBasketproduct.Counts += basketVM.SelectCount;
            //                basketVM.SelectCount = dbBasketproduct.Counts;
            //            }
            //        }
            //        else
            //        {
            //            Basket basket = new Basket
            //            {
            //                UserId = appuser.Id,
            //                ProductId = basketVM.ProductId,
            //                Counts = basketVM.SelectCount
            //            };

            //            baskets.Add(basket);
            //        }
            //    }
            //    basketCookie = JsonConvert.SerializeObject(basketVMs);

            //    HttpContext.Response.Cookies.Append("basket", basketCookie);
            //    //await _context.Baskets.AddRangeAsync(baskets);
            //    //await _context.SaveChangesAsync();
            //}
            //else
            //{
            //    if (appuser.Baskets != null && appuser.Baskets.Count() > 0)
            //    {
            //        List<CartProductCreateVM> basketVMs = new List<CartProductCreateVM>();

            //        foreach (Basket basket in appuser.Baskets)
            //        {
            //            CartProductCreateVM basketVM = new CartProductCreateVM
            //            {
            //                ProductId = basket.ProductId,
            //                SelectCount = basket.Counts
            //            };

            //            basketVMs.Add(basketVM);
            //        }

            //        basketCookie = JsonConvert.SerializeObject(basketVMs);

            //        HttpContext.Response.Cookies.Append("basket", basketCookie);
            //    }
            //}
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                    new { email = forgotPasswordVM.Email, token = token }, Request.Scheme);
                    _logger.Log(LogLevel.Warning, passwordResetLink);
                    return Redirect(passwordResetLink);
                }
                else
                {
                    ModelState.AddModelError("", "Email is incorrect");
                    return View();
                }
            }

            return View(forgotPasswordVM);
            
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid reset password token");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
                if (appUser != null)
                {
                    IdentityResult result = await _userManager.ResetPasswordAsync(appUser, resetPasswordVM.Token, resetPasswordVM.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordSuccessfully");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View();
                    }

                }
            }
            return View();
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Update(ProfileVM profileVM)
        {
            if (!ModelState.IsValid) return View("Profile", profileVM);

            AppUser dbAppUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (dbAppUser.NormalizedUserName != profileVM.UserName.Trim().ToUpperInvariant() ||
                dbAppUser.FullName.ToUpperInvariant() != profileVM.FullName.Trim().ToUpperInvariant() ||
                dbAppUser.NormalizedEmail != profileVM.Email.Trim().ToUpperInvariant())
            {
                dbAppUser.FullName = profileVM.FullName;
                dbAppUser.Email = profileVM.Email;
                dbAppUser.UserName = profileVM.UserName;

                IdentityResult identityResult = await _userManager.UpdateAsync(dbAppUser);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View("Profile", profileVM);
                }

                TempData["success"] = "Pr0fil Ugurla Yenilendi";
            }

            if (profileVM.CurrentPassword != null && profileVM.NewPassword != null)
            {
                if (await _userManager.CheckPasswordAsync(dbAppUser, profileVM.CurrentPassword) && profileVM.CurrentPassword == profileVM.NewPassword)
                {
                    ModelState.AddModelError("", "New Password Is The Same Current Password");
                    return View("Profile", profileVM);
                }

                IdentityResult identityResult = await _userManager.ChangePasswordAsync(dbAppUser, profileVM.CurrentPassword, profileVM.NewPassword);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View("Profile", profileVM);
                }

                TempData["successPassword"] = "Sifre Ugurla Yenilendi";
            }

            return RedirectToAction("Profile");
        }
    }
}
