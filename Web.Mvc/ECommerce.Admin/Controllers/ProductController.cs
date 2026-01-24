using App.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(products);
        }

        [HttpPost]
        public IActionResult Toggle(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Enabled = !product.Enabled;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
