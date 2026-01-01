using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Listing()
        {
            return View();
        }

        [Route("product/{categoryName}-{title}-{id}/details")]
        public IActionResult ProductDetail(string categoryName, string title, int id)
        {
            return View();
        }
    }
}
