using DiplomaProject_ITMO.Models;
using Microsoft.AspNetCore.Mvc;

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
                // 1. Расчет объема стен
                decimal wallVolume = (decimal)(model.Length * model.Width * model.Height);

                // 2. Расчет площади окон (учитываем вычет)
                decimal windowArea = (decimal)(model.WindowCount * 1.5 * 1.5);
                decimal totalMaterialVolume = wallVolume - windowArea; // Общий объем материала с вычетом окон

                // 3. Расчет стоимости материалов в зависимости от типа материала
                decimal materialCost = 0;
                switch (model.MaterialType)
                {
                    case "Wood":
                        // Для дерева стоимость указывается за м³
                        materialCost = (decimal)(model.MaterialCost * totalMaterialVolume);
                        break;
                    case "Brick":
                    case "GasBlock":
                        // Для кирпича и газоблока стоимость указывается за штуку.  Нужно приблизительно рассчитать кол-во штук.
                        // Это упрощенный пример, реальный расчет будет сложнее и зависеть от размеров кирпича/газоблока.

                        // Пример:  Предположим, что 1 м³ = 400 кирпичей/газоблоков (это нужно уточнить!).
                        decimal materialCount = totalMaterialVolume * 400;
                        materialCost = (decimal)(model.MaterialCost * materialCount);
                        break;
                    default:
                        ModelState.AddModelError("", "Неизвестный тип материала.");
                        return View("Index", model);  // Возвращаем представление с ошибкой
                }

                // 4. Расчет общей стоимости проекта
                decimal totalCost = (decimal)model.LandCost + materialCost;  // Учитываем стоимость участка и материалов

                // 5. Сохранение модели в базе данных
                model.TotalCost = totalCost; // Установка общей стоимости в модель
                _context.Projects.Add(model);

                try
                {
                    _context.SaveChanges(); // Используем правильный контекст
                }
                catch (Exception ex)
                {
                    // Обработка ошибки (например, логирование)
                    ModelState.AddModelError("", "Ошибка при сохранении данных: " + ex.Message);
                    return View("Index", model);
                }

                // 6. Передача данных в представление
                ViewBag.TotalMaterial = totalMaterialVolume;
                ViewBag.TotalCost = totalCost; // Передача общей стоимости в представление
                return View("Index", model);
            }

            return View("Index", model);
        }
    }
}
