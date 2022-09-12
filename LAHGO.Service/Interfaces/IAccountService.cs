using LAHGO.Service.ViewModels.AccountVMs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Service.Interfaces
{
    public interface IAccountService
    {
        Task Logout();
    }
}
