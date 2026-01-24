using App.Data.Context;
using App.Data.Entities;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = _db.Users.Any(x => x.Email == model.Email);
            if (exists)
            {
                ModelState.AddModelError("", "Bu email zaten kayıtlı.");
                return View(model);
            }

            var buyerRole = _db.Roles.FirstOrDefault(x => x.Name == "Buyer");
            if (buyerRole == null)
            {
                ModelState.AddModelError("", "Buyer rolü bulunamadı. (Seed verisi eksik olabilir)");
                return View(model);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password, 
                Enabled = false,
                RoleId = buyerRole.Id,
                CreatedAt = DateTime.Now
            };

            var selectedRole = _db.Roles.FirstOrDefault(r => r.Name == model.Role);

            if (selectedRole == null)
            {
                ModelState.AddModelError("", "Seçilen rol bulunamadı.");
                return View(model);
            }

            user.RoleId = selectedRole.Id;


            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _db.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Email veya şifre hatalı.");
                return View(model);
            }

            if (!user.Enabled)
            {
                ModelState.AddModelError("", "Hesabınız admin tarafından onaylanmadı.");
                return View(model);
            }

            var roleName = _db.Roles.FirstOrDefault(r => r.Id == user.RoleId)?.Name ?? "buyer";


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName.ToLower())
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Product");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
