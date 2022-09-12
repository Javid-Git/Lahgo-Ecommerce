using LAHGO.Core;
using LAHGO.Core.Entities;
using LAHGO.Service.Interfaces;
using LAHGO.Service.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Implementations
{
    public class AccountService : IAccountService
    {
        
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
       
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
