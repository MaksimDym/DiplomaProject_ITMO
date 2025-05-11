using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Если используете EF Core
using DiplomaProject_ITMO.Models; // Замените на ваше пространство имен для моделей (User.cs)

using System.Security.Cryptography; // Для хеширования
using System.Text; // Для хеширования
using Microsoft.AspNetCore.Http; // Для сессий

namespace DiplomaProject_ITMO.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context; // Замените ApplicationDbContext на ваш DbContext

        public AccountController(ApplicationDbContext context) // Замените ApplicationDbContext
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModelCustom()); // Используем вашу ViewModel
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModelCustom model) // Используем вашу ViewModel
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
        public async Task<IActionResult> Login(LoginViewModelCustom model) // Используем вашу ViewModel
        {
            if (ModelState.IsValid)
            {
                // В вашей LoginViewModelCustom поле называется UsernameOrEmail
                var user = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail
                );

                if (user != null && VerifyPassword(model.Password, user.PasswordHash)) // Ваш метод проверки пароля
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    // Дополнительно можно сохранять роль или другие данные в сессию, если нужно
                    // HttpContext.Session.SetString("UserRole", user.Role);

                    return RedirectToAction("Index", "Home"); // Перенаправление после успешного входа
                }

                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
            }
            return View(model); // Если модель не валидна или вход не удался, возвращаем форму с ошибками
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");
            // Удалите другие данные сессии, если вы их устанавливали
            // HttpContext.Session.Remove("UserRole");

            return RedirectToAction("Index", "Home"); // Перенаправление после выхода
        }


        // --- Вспомогательные методы для паролей (должны быть в вашем контроллере или сервисе) ---
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
                // Возвращаем хеш и соль вместе, разделенные, например, двоеточием
                return Convert.ToBase64String(hashedBytes) + ":" + salt;
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHashWithSalt)
        {
            if (string.IsNullOrEmpty(storedPasswordHashWithSalt) || !storedPasswordHashWithSalt.Contains(":"))
            {
                // Некорректный формат сохраненного хеша или хеш отсутствует
                return false;
            }

            var parts = storedPasswordHashWithSalt.Split(':');
            if (parts.Length != 2)
            {
                // Некорректный формат сохраненного хеша
                return false;
            }

            var storedHash = parts[0];
            var salt = parts[1];

            // Хешируем введенный пароль с извлеченной солью
            string hashedEnteredPasswordAttempt = HashPassword(enteredPassword, salt).Split(':')[0];

            return storedHash == hashedEnteredPasswordAttempt;
        }
    }
}