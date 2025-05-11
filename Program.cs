using DiplomaProject_ITMO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;


var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllersWithViews();

// 2. Регистрация ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

// 3. Регистрация сервисов для сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время простоя сессии
    options.Cookie.HttpOnly = true;                 // Cookie доступен только через HTTP (не через JS)
    options.Cookie.IsEssential = true;              // Cookie необходим для работы основных функций приложения
});

// Если у вас есть другие сервисы (например, EmailService), их нужно регистрировать здесь:
// builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// builder.Services.AddTransient<IEmailService, EmailService>();


// --- Построение приложения ---
var app = builder.Build();


// --- Конфигурация конвейера HTTP-запросов (Middleware) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Значение HSTS по умолчанию — 30 дней. Вы можете изменить это для производственных сценариев.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Полезно для отладки в режиме разработки
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();