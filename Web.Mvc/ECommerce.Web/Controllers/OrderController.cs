using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
