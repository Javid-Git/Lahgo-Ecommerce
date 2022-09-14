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

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
