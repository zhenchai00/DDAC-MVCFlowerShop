using Microsoft.AspNetCore.Mvc;
using MVCFlowerShop.Models;
using MVCFlowerShop.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MVCFlowerShop.Controllers
{
    public class FlowerController : Controller
    {
        private readonly MVCFlowerShopContext _context;

        public FlowerController(MVCFlowerShopContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Index()
        {
            List<FlowerTable> flowers = await _context.FlowerList.ToListAsync();
            return View(flowers);
        }

        public IActionResult AddData()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddData(FlowerTable flower)
        {
            if (ModelState.IsValid)
            {
                _context.FlowerList.Add(flower);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(flower);
        }
    }
}
