using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Admin.Controllers
{
    public class UserController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Approve(int id)
        {
            return View();
        }
    }
}
