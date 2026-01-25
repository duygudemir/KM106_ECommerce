using ECommerce.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using App.Data.Context;
using Microsoft.AspNetCore.Authorization;


namespace ECommerce.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.UserCount = _db.Users.Count();
            ViewBag.ProductCount = _db.Products.Count();
            ViewBag.PendingCommentCount = _db.ProductComments.Count(x => !x.IsConfirmed);

            return View();
        }
    }
}
