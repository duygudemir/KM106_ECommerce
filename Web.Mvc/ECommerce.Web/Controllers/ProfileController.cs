using App.Data.Context;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly AppDbContext _db;

        public ProfileController(AppDbContext db)
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
        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Index()
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == CurrentUserId);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _db.Users.FirstOrDefault(x => x.Id == CurrentUserId);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var emailExists = _db.Users.Any(x => x.Email == model.Email && x.Id != user.Id);
            if (emailExists)
            {
                ModelState.AddModelError("", "Bu email başka bir kullanıcı tarafından kullanılıyor.");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            _db.SaveChanges();

            ViewBag.Success = "Profil güncellendi ✅";
            return View(model);
        }
    }
}
