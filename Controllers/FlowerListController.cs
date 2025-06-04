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

        public async Task<IActionResult> EditData(int? FlowerId)
        {
            if (FlowerId == null)
            {
                return NotFound();
            }
            var flower = await _context.FlowerList.FindAsync(FlowerId);

            if (flower == null)
            {
                return BadRequest(FlowerId + " is not found in the table!");
            }

            return View(flower);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateData(FlowerTable flower)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.FlowerList.Update(flower);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "FlowerList");
                }
                return View("EditData", flower);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        public async Task<IActionResult> DeleteData(int? FlowerId)
        {
            if (FlowerId == null)
            {
                return NotFound();
            }

            var flower = await _context.FlowerList.FindAsync(FlowerId);

            if (flower == null)
            {
                return BadRequest(FlowerId + " is not found in the list!");
            }

            _context.FlowerList.Remove(flower);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FlowerList");
        }
    }
}
