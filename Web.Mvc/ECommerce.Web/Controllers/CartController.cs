using App.Data.Context;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        private int CurrentUserId => 1;

        public IActionResult Index()
        {
            var cart = _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == CurrentUserId && !c.IsCompleted);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = CurrentUserId,
                    CreatedAt = DateTime.Now,
                    TotalPrice = 0
                };

                _db.Carts.Add(cart);
                _db.SaveChanges();
            }

            return View(cart);
        }

        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 5) quantity = 5;

            var product = _db.Products.FirstOrDefault(p => p.Id == productId && p.Enabled);
            if (product == null)
                return NotFound();

            var cart = _db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == CurrentUserId && !c.IsCompleted);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = CurrentUserId,
                    CreatedAt = DateTime.Now
                };

                _db.Carts.Add(cart);
                _db.SaveChanges();
            }

            var existingItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                var newQty = existingItem.Quantity + quantity;
                if (newQty > 5) newQty = 5;
                existingItem.Quantity = (byte)newQty;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = (byte)quantity
                });
            }

            RecalculateCartTotal(cart.Id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity < 1) quantity = 1;
            if (quantity > 5) quantity = 5;

            var item = _db.CartItems.FirstOrDefault(x => x.Id == cartItemId);
            if (item == null)
                return NotFound();

            item.Quantity = (byte)quantity;
            _db.SaveChanges();

            RecalculateCartTotal(item.CartId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int cartItemId)
        {
            var item = _db.CartItems.FirstOrDefault(x => x.Id == cartItemId);
            if (item == null)
                return NotFound();

            var cartId = item.CartId;

            _db.CartItems.Remove(item);
            _db.SaveChanges();

            RecalculateCartTotal(cartId);

            return RedirectToAction("Index");
        }

        private void RecalculateCartTotal(int cartId)
        {
            var cart = _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.Id == cartId);

            if (cart == null) return;

            cart.TotalPrice = cart.CartItems.Sum(ci => (ci.Product != null ? ci.Product.Price : 0) * ci.Quantity);
            _db.SaveChanges();
        }
    }
}
