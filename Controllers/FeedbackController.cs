using DiplomaProject_ITMO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DiplomaProject_ITMO.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new FeedbackModel()); // передаем пустую модель
        }

        [HttpPost]
        public async Task<IActionResult> Submit(FeedbackModel feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ваше сообщение успешно отправлено!";
                return RedirectToAction("ThankYou");
            }
            return View("Index", feedback);
        }

        public IActionResult ThankYou()
        {
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            return View();
        }
    }
}