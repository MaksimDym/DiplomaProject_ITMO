using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using DiplomaProject_ITMO.Models; 

using System.Security.Cryptography;
using System.Text; 
using Microsoft.AspNetCore.Http; 

namespace DiplomaProject_ITMO.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModelCustom()); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModelCustom model) 
        {
            if (ModelState.IsValid)
            {
               
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Это имя пользователя уже занято.");
                }
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Этот адрес электронной почты уже зарегистрирован.");
                }

                if (ModelState.ErrorCount > 0) 
                {
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password), 
                    
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

            

                ViewBag.SuccessMessage = "Регистрация прошла успешно! Теперь вы можете войти.";
                return View("Login", new LoginViewModelCustom()); 
            }
            return View(model); 
        }

       
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModelCustom()); 
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModelCustom model) 
        {
            if (ModelState.IsValid)
            {
                
                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail
                );

                if (user != null && VerifyPassword(model.Password, user.PasswordHash)) 
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                   

                    return RedirectToAction("Index", "Home"); 
                }

                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
            }
            return View(model); 
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");
            

            return RedirectToAction("Index", "Home");
        }


      
        private string HashPassword(string password, string salt = null)
        {
            bool newSaltGenerated = false;
            byte[] saltBytes;

            if (string.IsNullOrEmpty(salt))
            {
                saltBytes = new byte[16];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(saltBytes);
                }
                salt = Convert.ToBase64String(saltBytes);
                newSaltGenerated = true;
            }
            else
            {
                saltBytes = Convert.FromBase64String(salt);
            }

            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt; // Простой способ конкатенации, для продакшена рассмотрите HMAC
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
              
                return Convert.ToBase64String(hashedBytes) + ":" + salt;
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHashWithSalt)
        {
            if (string.IsNullOrEmpty(storedPasswordHashWithSalt) || !storedPasswordHashWithSalt.Contains(":"))
            {
               
                return false;
            }

            var parts = storedPasswordHashWithSalt.Split(':');
            if (parts.Length != 2)
            {
              
                return false;
            }

            var storedHash = parts[0];
            var salt = parts[1];


            string hashedEnteredPasswordAttempt = HashPassword(enteredPassword, salt).Split(':')[0];

            return storedHash == hashedEnteredPasswordAttempt;
        }
    }
}