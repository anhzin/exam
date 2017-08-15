using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Test.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return RedirectToAction("Login", "User",null);
        }

        public IActionResult Register()
        {
            return RedirectToAction("Create", "User", null);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
