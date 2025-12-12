using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OnlineBookStors.Data;
using OnlineBookStors.Models;

namespace OnlineBookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext context) => _context = context;

        private int CurrentUserId =>
            int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        public IActionResult Index()
        {
            var items = _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.Id == CurrentUserId)
                .ToList();

            return View(items);
        }

        public IActionResult Add(int bookId)
        {
            var existing = _context.CartItems
                .FirstOrDefault(c => c.BookId == bookId && c.Id == CurrentUserId);

            if (existing != null)
                existing.Quantity++;
            else
                _context.CartItems.Add(new CartItem
                {
                    BookId = bookId,
                    Id = CurrentUserId,
                    Quantity = 1
                });

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var item = _context.CartItems.Find(id);
            if (item != null) _context.CartItems.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id, int qty)
        {
            var item = _context.CartItems.Find(id);
            if (item != null) item.Quantity = qty;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}



