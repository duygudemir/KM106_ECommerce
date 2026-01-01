using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
