using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiplomaProject_ITMO.Models; 
using System.Threading.Tasks;
using System.Security.Cryptography; 
using System.Text;
using Microsoft.AspNetCore.Http; 

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    private string HashPassword(string password, string salt = null)
    {
        if (string.IsNullOrEmpty(salt))
        {
          
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);
        }

       
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + salt; 
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
        if (parts.Length != 2) return false;

        var storedHash = parts[0];
        var salt = parts[1];

       
        string hashedEnteredPasswordWithStoredSalt = HashPassword(enteredPassword, salt);
        var enteredHash = hashedEnteredPasswordWithStoredSalt.Split(':')[0]; 
        return storedHash == enteredHash;
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
                return View(model);
            }
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Этот адрес электронной почты уже зарегистрирован.");
                return View(model);
            }

            // Хешируем пароль (HashPassword сгенерирует новую соль)
            var passwordHashWithSalt = HashPassword(model.Password);

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHashWithSalt, 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home");
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail);

            if (user != null)
            {
               
                if (VerifyPassword(model.Password, user.PasswordHash))
                {
                    
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("Username", user.Username);
                    
                    return RedirectToAction("Index", "Home");
                }
            }

          
            ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
            return View(model);
        }
        return View(model);
    }


    [HttpPost] 
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserId");
        HttpContext.Session.Remove("Username");
       
        return RedirectToAction("Index", "Home");
    }
}