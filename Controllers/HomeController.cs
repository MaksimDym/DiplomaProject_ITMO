using System.Diagnostics;
using DiplomaProject_ITMO.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject_ITMO.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

