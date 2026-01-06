using App.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult List()
        {
            var users = _db.Users
                .Include(u => u.Role)
                .OrderByDescending(u => u.Id)
                .ToList();

            return View(users);
        }

        public IActionResult Approve(int id)
        {
            var user = _db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult ApproveConfirmed(int id)
        {
            var user = _db.Users.Find(id);

            if (user == null)
                return NotFound();

            user.Enabled = true;
            _db.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
