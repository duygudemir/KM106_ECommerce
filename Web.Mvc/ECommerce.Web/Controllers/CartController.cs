using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult AddProduct(int productId)
        {
            return View();
        }

        public IActionResult Edit(int cartItemId)
        {
            return View();
        }
    }
}
