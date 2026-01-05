using App.Data.Context;
using Microsoft.AspNetCore.Mvc;
using App.Data.Entities;
using ECommerce.Admin.Models;


namespace ECommerce.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult List()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var category = new Category
            {
                Name = vm.Name,
                Color = vm.Color,
                IconCssClass = vm.IconCssClass,
                CreatedAt = DateTime.Now
            };

            _db.Categories.Add(category);
            _db.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult Edit(int id)
        {
            var category = _db.Categories.Find(id);

            if (category == null)
                return NotFound();

            var vm = new CategoryEditVm
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color,
                IconCssClass = category.IconCssClass
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var category = _db.Categories.Find(vm.Id);

            if (category == null)
                return NotFound();

            category.Name = vm.Name;
            category.Color = vm.Color;
            category.IconCssClass = vm.IconCssClass;

            _db.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _db.Categories.Find(id);

            if (category == null)
                return NotFound();

            _db.Categories.Remove(category);
            _db.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
