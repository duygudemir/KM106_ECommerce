using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ProductController : Controller
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

        public IActionResult Comment(int productId)
        {
            return View();
        }
    }
}
