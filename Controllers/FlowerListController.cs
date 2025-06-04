using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCFlowerShop.Models;
using Microsoft.EntityFrameworkCore; // data connection 
using MVCFlowerShop.Data; // find the data connection file

namespace MVCFlowerShop.Controllers
{
    public class FlowerListController : Controller
    {
        private readonly MVCFlowerShopContext _context;     // create a data member for calling the connection
        
        // constructor
        public FlowerListController(MVCFlowerShopContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Index() // display table in index
        {
            // get list of flower from table 
            List<FlowerTable> flowerList = await _context.FlowerList.ToListAsync();
            return View(flowerList);
        }

        public IActionResult AddData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // avoid cross site attack
        public async Task<IActionResult> AddData(FlowerTable flower)
        {
            if(ModelState.IsValid) // form is no error
            {
                _context.Add(flower); // bring flower details to table object
                await _context.SaveChangesAsync(); // commit action
                return RedirectToAction("Index", "FlowerList"); // return to index page
            }

            return View(flower);    // form is error, then direct to previous page and display error data
        }
    }
}
