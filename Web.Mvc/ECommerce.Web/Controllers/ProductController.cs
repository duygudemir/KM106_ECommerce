using App.Data.Context;
using App.Data.Entities;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name", vm.CategoryId);
                return View(vm);
            }

            
            var sellerId = 1;

            var product = new Product
            {
                SellerId = sellerId,
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Price = vm.Price,
                Details = vm.Details,
                StockAmount = (byte)vm.StockAmount,
                CreatedAt = DateTime.Now,
                Enabled = true
            };

            _db.Products.Add(product);
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        public IActionResult MyProducts()
        {
            var sellerId = 1;

            var products = _db.Products
                .Where(p => p.SellerId == sellerId)
                .ToList();

            return View(products);
        }

        public IActionResult EditPrice(int id)
        {
            var sellerId = 1;

            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == sellerId);
            if (product == null)
                return NotFound();

            var vm = new ProductEditPriceVm
            {
                Id = product.Id,
                Price = product.Price
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditPrice(ProductEditPriceVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var sellerId = 1;

            var product = _db.Products.FirstOrDefault(p => p.Id == vm.Id && p.SellerId == sellerId);
            if (product == null)
                return NotFound();

            product.Price = vm.Price;
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        public IActionResult EditStock(int id)
        {
            var sellerId = 1;

            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == sellerId);
            if (product == null)
                return NotFound();

            var vm = new ProductEditStockVm
            {
                Id = product.Id,
                StockAmount = product.StockAmount
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditStock(ProductEditStockVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var sellerId = 1;

            var product = _db.Products.FirstOrDefault(p => p.Id == vm.Id && p.SellerId == sellerId);
            if (product == null)
                return NotFound();

            product.StockAmount = (byte)vm.StockAmount;
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        [HttpPost]
        public IActionResult ToggleEnabled(int id)
        {
            var sellerId = 1;

            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == sellerId);
            if (product == null)
                return NotFound();

            product.Enabled = !product.Enabled;
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }


        public IActionResult Delete(int id)
        {
            var sellerId = 1;

            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == sellerId);
            if (product == null)
                return NotFound();

            return View(product);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var sellerId = 1;

            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == sellerId);
            if (product == null)
                return NotFound();

            _db.Products.Remove(product);
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        public IActionResult Index()
        {
            var products = _db.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Where(p => p.Enabled)
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(products);
        }


        public IActionResult Comment(int productId)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == productId && p.Enabled);
            if (product == null)
                return NotFound();

            var vm = new ProductCommentCreateVm
            {
                ProductId = productId
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Comment(ProductCommentCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            
            var currentUserId = 1;

            var comment = new ProductComment
            {
                ProductId = vm.ProductId,
                UserId = currentUserId,
                Text = vm.Text,
                StarCount = (byte)vm.StarCount,
                IsConfirmed = false,
                CreatedAt = DateTime.Now
            };

            _db.ProductComments.Add(comment);
            _db.SaveChanges();

            return RedirectToAction("Details", new { id = vm.ProductId });
        }
    }
}
