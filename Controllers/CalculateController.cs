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
     
                decimal perimeter = (decimal)(2 * (model.Length + model.Width));
                decimal wallHeight = (decimal)model.Height;
                decimal wallArea = perimeter * wallHeight;
                decimal windowArea = (decimal)(model.WindowCount * 1.4 * 1.4);
                decimal netWallArea = wallArea - windowArea;

                decimal materialCost = 0;
                switch (model.MaterialType)
                {
                    case "Wood":
                        decimal woodVolume = netWallArea * 0.2M;
                        materialCost = (decimal)(model.MaterialCost * woodVolume);
                        break;

                    case "GasBlock":
                        decimal gasBlockWidth = 0.1M;
                        decimal gasBlockHeight = 0.25M;
                        decimal gasBlockLength = 0.625M;
                        decimal gasBlockArea = gasBlockLength * gasBlockHeight;
                        decimal gasBlockPerSquareMeter = 1 / gasBlockArea;
                        decimal gasBlockCount = netWallArea * gasBlockPerSquareMeter;
                        materialCost = (decimal)(model.MaterialCost * gasBlockCount);
                        break;

                    case "Brick":
                        decimal brickLength = 0.25M;
                        decimal brickWidth = 0.12M * 2;
                        decimal brickHeight = 0.065M;
                        decimal brickArea = brickLength * brickHeight;
                        decimal brickPerSquareMeter = 1 / brickArea;
                        decimal brickCount = netWallArea * brickPerSquareMeter;
                        materialCost = (decimal)(model.MaterialCost * brickCount);
                        break;

                    default:
                        ModelState.AddModelError("", "Неизвестный тип материала.");
                        return View("Index", model);
                }

                
                decimal foundationCost = 0;
                switch (model.FoundationType)
                {
                    case "Slab":
                        
                        foundationCost = (decimal)(10000 * model.Length * model.Width*500); 
                        break;
                    case "Strip":
                        
                        foundationCost = (decimal)(500 * perimeter); 
                        break;
                    case "Pile":
                       
                        foundationCost = (decimal)(5000 * 20); 
                        break;
                    default:
                        ModelState.AddModelError("", "Неизвестный тип фундамента.");
                        return View("Index", model);
                }
                decimal saunaCost = model.HasSauna ? 50000 : 0;
                decimal fenceCost = model.HasFence ? 30000 : 0; 
                decimal electricityCost = model.NeedsElectricity ? 20000 : 0; 
                decimal waterSupplyCost = model.HasWaterSupply ? 25000 : 0;
                decimal totalCost = (decimal)model.LandCost + materialCost + foundationCost +
                                    saunaCost + fenceCost + electricityCost + waterSupplyCost;
                model.TotalCost = totalCost;
                _context.Projects.Add(model);

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при сохранении данных: " + ex.Message);
                    return View("Index", model);
                }
                ViewBag.TotalMaterial = netWallArea;
                ViewBag.TotalCost = totalCost;
                return View("Index", model);
            }
            return View("Index", model);
        }
    }
}