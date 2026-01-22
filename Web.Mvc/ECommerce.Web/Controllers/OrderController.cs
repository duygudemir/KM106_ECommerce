using App.Data.Context;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
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

        public IActionResult Create()
        {
            var cart = _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == CurrentUserId && !c.IsCompleted);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return RedirectToAction("Index", "Cart");

            var order = new Order
            {
                UserId = CurrentUserId,
                CreatedAt = DateTime.Now,
                TotalPrice = cart.TotalPrice,
                OrderItems = new List<OrderItem>() 
            };

            foreach (var item in cart.CartItems)
            {
                if (item.Product == null) continue;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });
            }

            _db.Orders.Add(order);

            cart.IsCompleted = true;

            _db.SaveChanges();

            return RedirectToAction("MyOrders");
        }

        public IActionResult MyOrders()
        {
            var orders = _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == CurrentUserId) 
                .OrderByDescending(o => o.Id)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == id && o.UserId == CurrentUserId);

            if (order == null)
                return NotFound();

            order.IsCancelled = true;
            _db.SaveChanges();

            return RedirectToAction("MyOrders");
        }

    }
}
