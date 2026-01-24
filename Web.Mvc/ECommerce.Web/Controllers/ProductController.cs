using App.Data.Context;
using App.Data.Entities;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        private int CurrentUserId
        {
            get
            {
                var idText = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.Parse(idText!);
            }
        }

        [Authorize(Roles = "seller")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "seller")]
        [HttpPost]
        public IActionResult Create(ProductCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name", vm.CategoryId);
                return View(vm);
            }

            var product = new Product
            {
                SellerId = CurrentUserId, 
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

        [Authorize(Roles = "seller")]
        public IActionResult MyProducts()
        {
            var products = _db.Products
                .Where(p => p.SellerId == CurrentUserId) 
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(products);
        }

        [Authorize(Roles = "seller")]
        public IActionResult EditPrice(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == CurrentUserId);
            if (product == null)
                return NotFound();

            var vm = new ProductEditPriceVm
            {
                Id = product.Id,
                Price = product.Price
            };

            return View(vm);
        }

        [Authorize(Roles = "seller")]
        [HttpPost]
        public IActionResult EditPrice(ProductEditPriceVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var product = _db.Products.FirstOrDefault(p => p.Id == vm.Id && p.SellerId == CurrentUserId);
            if (product == null)
                return NotFound();

            product.Price = vm.Price;
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        [Authorize(Roles = "seller")]
        public IActionResult EditStock(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == CurrentUserId);
            if (product == null)
                return NotFound();

            var vm = new ProductEditStockVm
            {
                Id = product.Id,
                StockAmount = product.StockAmount
            };

            return View(vm);
        }

        [Authorize(Roles = "seller")]
        [HttpPost]
        public IActionResult EditStock(ProductEditStockVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var product = _db.Products.FirstOrDefault(p => p.Id == vm.Id && p.SellerId == CurrentUserId);
            if (product == null)
                return NotFound();

            product.StockAmount = (byte)vm.StockAmount;
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        [Authorize(Roles = "seller")]
        [HttpPost]
        public IActionResult ToggleEnabled(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == CurrentUserId);
            if (product == null)
                return NotFound();

            product.Enabled = !product.Enabled;
            _db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        [Authorize(Roles = "seller")]
        public IActionResult Delete(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == CurrentUserId);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [Authorize(Roles = "seller")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id && p.SellerId == CurrentUserId);
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

        [Authorize(Roles = "buyer")]
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

        [Authorize] 
        [HttpPost]
        public IActionResult Comment(ProductCommentCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdStr);

            var comment = new ProductComment
            {
                ProductId = vm.ProductId,
                UserId = userId,
                Text = vm.Text,
                StarCount = (byte)vm.StarCount,
                IsConfirmed = false,
                CreatedAt = DateTime.Now
            };

            _db.ProductComments.Add(comment);
            _db.SaveChanges();

            return RedirectToAction("Details", new { id = vm.ProductId });
        }

        public IActionResult Details(int id)
        {
            var product = _db.Products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .FirstOrDefault(p => p.Id == id && p.Enabled);

            if (product == null)
                return NotFound();

            var comments = _db.ProductComments
                .Include(c => c.User)
                .Where(c => c.ProductId == id && c.IsConfirmed)
                .OrderByDescending(c => c.Id)
                .ToList();

            ViewBag.Comments = comments;
            return View(product);
        }
    }
}
