using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OnlineBookStors.Data;
using OnlineBookStors.Models;

namespace OnlineBookStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context) => _context = context;

        private int CurrentUserId =>
            int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        public IActionResult Checkout()
        {
            var items = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.Id == CurrentUserId)
                .ToList();

            return View(items);
        }

        public IActionResult PlaceOrder()
        {
            var cartItems = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.Id == CurrentUserId)
                .ToList();

            var order = new Order
            {
                UserId = CurrentUserId,
                Created = DateTime.Now,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Book!.Price
                });
            }

            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction("MyOrders");
        }

        public IActionResult MyOrders()
        {
            var orders = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .Where(o => o.UserId == CurrentUserId)
                .ToList();

            return View(orders);
        }

        public IActionResult Cancel(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null && order.Status == "Pending")
            {
                order.Status = "Cancelled";
                _context.SaveChanges();
            }
            return RedirectToAction("MyOrders");
        }
    }
}



