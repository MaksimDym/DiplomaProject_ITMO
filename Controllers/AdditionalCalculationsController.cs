using DiplomaProject_ITMO.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaProject_ITMO.Controllers
{
    public class AdditionalCalculationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdditionalCalculationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(AdditionalCalculationsModel model)
        {
            // Логика дополнительных расчетов
            _context.AdditionalCalculations.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
