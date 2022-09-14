using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.AccountVMs;
using LAHGO.Service.ViewModels.CartProductVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration, IAccountService accountService, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accountService = accountService;
            _configuration = configuration;
        }
        
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
                ModelState.AddModelError("", $"{registerVM.FullName}");
                return View();
            }
            AppUser appUser = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };
            string token = Guid.NewGuid().ToString();
            appUser.ConfirmationToken = token;

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);


            var link = Url.Action(nameof(ConfirmEmail), "Account", new { id = appUser.Id, token = appUser.ConfirmationToken }, Request.Scheme, Request.Host.ToString());
            EmailVM email = _configuration.GetSection("Email").Get<EmailVM>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(appUser.Email);
            mail.Subject = "Account activation";
            mail.Body = $"<a href=\"{link}\">Confirmation link</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.EnableSsl = true;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.SenderPassword);
            smtp.Send(mail);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            result = await _userManager.AddToRoleAsync(appUser, "User");
            return RedirectToAction("ConfirmPage");

        }



        #region Reset Password
        [HttpGet]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM reset)
        {
            if (string.IsNullOrWhiteSpace(reset.Email))
            {
                ModelState.AddModelError(string.Empty, "Please enter valid e-mail adress");
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(reset.Email);

            

            user.PasswordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _unitOfWork.CommitAsync();

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Account wasn't found");
                return View();
            }
            var link = Url.Action(nameof(NewPassword), "Account", new { id = user.Id, token = user.PasswordResetToken }, Request.Scheme, Request.Host.ToString());
            EmailVM email = _configuration.GetSection("Email").Get<EmailVM>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(reset.Email);
            mail.Subject = "Reset Password";
            mail.Body = $"<a href=\"{link}\">Reset Password</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.EnableSsl = true;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.SenderPassword);
            smtp.Send(mail);

            return RedirectToAction(nameof(ResetPage));
        }
        public async Task<IActionResult> ResetPage()
        {
            return View();
        }


        [HttpGet]
        public IActionResult NewPassword(ResetPasswordVM reset)
        {
            return View(reset);
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(ResetPasswordVM reset, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }
            if (reset.Id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(reset.Id);

            if (user == null)
            {
                return NotFound();
            }
            if (user.PasswordResetToken != reset.Token)
            {
                return BadRequest();
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, reset.Password);


            if (result.Succeeded)
            {
                string passwordResetToken = Guid.NewGuid().ToString();
                user.PasswordResetToken = passwordResetToken;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Login");
            }
            return BadRequest();
        }

        #endregion


        #region Email Confirmation
        public async Task<IActionResult> ConfirmPage()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Not Found");
            }
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                throw new Exception("Not Found");

            }
            if (appUser.ConfirmationToken != token)
            {
                throw new Exception("Bad Request");

            }

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            IdentityResult result = await _userManager.ConfirmEmailAsync(appUser, confirmationToken);

            if (result.Succeeded)
            {
                string newToken = Guid.NewGuid().ToString();
                appUser.ConfirmationToken = newToken;
                await _userManager.UpdateAsync(appUser);
                appUser.EmailConfirmed = true;
                return View();
            }
            return View();
        }
        #endregion



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
            if (!appuser.EmailConfirmed)
            {
                ModelState.AddModelError("", "The email confirmation is requested");

                return View(login);
            }
            await _signInManager.SignInAsync(appuser, (bool)login.RememberMe);
            string basketCookie = HttpContext.Request.Cookies["basket"];
            List<CartProductCreateVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductCreateVM>>(basketCookie);
            List<Basket> userbasket = await _unitOfWork.BasketRepository.GetAllAsync(b => b.UserId == appuser.Id);

            if (!string.IsNullOrWhiteSpace(basketCookie) && basketVMs.Count != 0)
            {
                List<Basket> baskets = new List<Basket>();
                foreach (CartProductCreateVM basketVM in basketVMs)
                {
                    if (userbasket != null && userbasket.Count() > 0)
                    {
                        Basket dbBasketproduct = userbasket.FirstOrDefault(b => b.ProductId != basketVM.ProductId);

                        if (dbBasketproduct == null)
                        {
                            Basket basket = new Basket
                            {
                                UserId = appuser.Id,
                                ProductId = basketVM.ProductId,
                                Counts = basketVM.SelectCount,
                                SizeId = basketVM.SizeId,
                                ColorId = basketVM.ColorId
                            };

                            baskets.Add(basket);
                        }
                        else
                        {
                            //exsitedBasket.Count = basketVM.Count;
                            dbBasketproduct.Counts += basketVM.SelectCount;
                            basketVM.SelectCount = dbBasketproduct.Counts;
                        }
                    }
                    else
                    {
                        Basket basket = new Basket
                        {
                            UserId = appuser.Id,
                            ProductId = basketVM.ProductId,
                            Counts = basketVM.SelectCount,
                            SizeId = basketVM.SizeId,
                            ColorId = basketVM.ColorId
                        };

                        baskets.Add(basket);
                    }
                }
                basketCookie = JsonConvert.SerializeObject(basketVMs);

                HttpContext.Response.Cookies.Append("basket", basketCookie);
                await _unitOfWork.BasketRepository.AddAllAsync(baskets);
                await _unitOfWork.CommitAsync();
            }
            else
            {

                if (userbasket != null && userbasket.Count() > 0)
                {
                    List<CartProductCreateVM> basketVMss = new List<CartProductCreateVM>();

                    foreach (Basket basket in userbasket)
                    {
                        CartProductCreateVM basketVM = new CartProductCreateVM
                        {
                            ProductId = basket.ProductId,
                            SelectCount = basket.Counts,
                            SizeId = basket.SizeId,
                            ColorId = basket.ColorId
                        };

                        basketVMss.Add(basketVM);
                    }

                    basketCookie = JsonConvert.SerializeObject(basketVMss);

                    HttpContext.Response.Cookies.Append("basket", basketCookie);
                }
            }
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            string basketcookie = HttpContext.Request.Cookies["basket"];
            List<CartProductGetVM> basketVMs = JsonConvert.DeserializeObject<List<CartProductGetVM>>(basketcookie);

            AppUser appuser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            foreach (var basket in basketVMs.ToList())
            {
                basketVMs.Remove(basket);

                basketcookie = JsonConvert.SerializeObject(basketVMs);
                HttpContext.Response.Cookies.Append("basket", basketcookie);
            }
            await _accountService.Logout();
            return RedirectToAction("index", "home");
        }



        public async Task<IActionResult> Profile()
        {
            AppUser appUser = await _userManager.Users
                .Include(u => u.Orders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (appUser == null) return NotFound();

            ProfileVM profileVM = new ProfileVM
            {
                FullName = appUser.FullName,
                Email = appUser.Email,
            };

            MemberVM memberVM = new MemberVM
            {
                ProfileVM = profileVM,
                Orders = appUser.Orders
            };

            return View(memberVM);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Update(ProfileVM profileVM)
        {

            AppUser dbAppUser = await _userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<Order> orders = await _unitOfWork.OrderRepository.GetAllAsyncInclude(o => o.AppUserId == dbAppUser.Id, "OrderItems", "OrderItems.Product");

            MemberVM memberVM = new MemberVM
            {
                Orders = orders,
                ProfileVM = profileVM
            };
            if (!ModelState.IsValid) return View("Profile", memberVM);

            if (dbAppUser.FullName.ToUpperInvariant() != profileVM.FullName.Trim().ToUpperInvariant() ||
                dbAppUser.NormalizedEmail != profileVM.Email.Trim().ToUpperInvariant())
            {
                dbAppUser.FullName = profileVM.FullName;
                dbAppUser.Email = profileVM.Email;

                IdentityResult identityResult = await _userManager.UpdateAsync(dbAppUser);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View("Profile", memberVM);
                }

                TempData["success"] = "Pr0fil Ugurla Yenilendi";
            }

            if (profileVM.CurrentPassword != null && profileVM.NewPassword != null)
            {
                if (await _userManager.CheckPasswordAsync(dbAppUser, profileVM.CurrentPassword) && profileVM.CurrentPassword == profileVM.NewPassword)
                {
                    ModelState.AddModelError("", "New Password Is The Same Current Password");
                    return View("Profile", memberVM);
                }

                IdentityResult identityResult = await _userManager.ChangePasswordAsync(dbAppUser, profileVM.CurrentPassword, profileVM.NewPassword);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View("Profile", memberVM);
                }

                TempData["successPassword"] = "Sifre Ugurla Yenilendi";
            }
            await _unitOfWork.CommitAsync();
            
            return RedirectToAction("Profile", memberVM);
        }



        //[HttpGet]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        AppUser user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
        //        if (user != null)
        //        {
        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            var passwordResetLink = Url.Action("ResetPassword", "Account",
        //            new { email = forgotPasswordVM.Email, token = token }, Request.Scheme);
        //            _logger.Log(LogLevel.Warning, passwordResetLink);
        //            return Redirect(passwordResetLink);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Email is incorrect");
        //            return View();
        //        }
        //    }

        //    return View(forgotPasswordVM);

        //}

        //[HttpGet]
        //public IActionResult ResetPassword(string token, string email)
        //{
        //    if (token == null || email == null)
        //    {
        //        ModelState.AddModelError("", "Invalid reset password token");
        //    }
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        AppUser appUser = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
        //        if (appUser != null)
        //        {
        //            IdentityResult result = await _userManager.ResetPasswordAsync(appUser, resetPasswordVM.Token, resetPasswordVM.Password);
        //            if (result.Succeeded)
        //            {
        //                return View("ResetPasswordSuccessfully");
        //            }
        //            else
        //            {
        //                foreach (IdentityError error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }
        //                return View();
        //            }

        //        }
        //    }
        //    return View();
        //}

    }
}
