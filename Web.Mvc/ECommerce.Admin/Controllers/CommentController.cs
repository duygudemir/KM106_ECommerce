using App.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Admin.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext _db;

        public CommentController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult List()
        {
            var comments = _db.ProductComments
                .Include(c => c.Product)
                .Include(c => c.User)
                .OrderByDescending(c => c.Id)
                .ToList();

            return View(comments);
        }

        public IActionResult Approve(int id)
        {
            var comment = _db.ProductComments
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == id);

            if (comment == null)
                return NotFound();

            return View(comment);
        }

        [HttpPost]
        public IActionResult ApproveConfirmed(int id)
        {
            var comment = _db.ProductComments.Find(id);

            if (comment == null)
                return NotFound();

            comment.IsConfirmed = true;
            _db.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
