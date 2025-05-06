using DiplomaProject_ITMO.Models;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }

        [HttpPost]
        public IActionResult Submit(FeedbackModel feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                return RedirectToAction("ThankYou");
            }
            return View("Index", feedback);
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
