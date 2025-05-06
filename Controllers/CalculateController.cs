using DiplomaProject_ITMO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DiplomaProject_ITMO.Controllers
{
    public class CalculateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalculateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                
                decimal totalMaterial = (model.Length * model.Width * model.Height) - (model.WindowCount * 1.5m); 
                                                                                                                  
                _context.Projects.Add(model);
                _context.SaveChanges();

                ViewBag.TotalMaterial = totalMaterial;
                return View("Index", model);
            }
            return View("Index", model);
        }
    }
}
