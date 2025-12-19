using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Security.Claims;

namespace OnlineBookStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var items = await _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.AppUserId == userId)
                .ToListAsync();

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int bookId, int quantity = 1)
        {
            var userId = GetUserId();
            var book = await _context.Books.FindAsync(bookId);

            if (book == null) return NotFound();

            if (quantity > book.Stock)
            {
                TempData["Error"] = $"Only {book.Stock} copies available.";
                return RedirectToAction("Index", "Book");
            }

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(c => c.AppUserId == userId && c.BookId == bookId);

            if (existing != null)
            {
                if (existing.Quantity + quantity > book.Stock)
                {
                    TempData["Error"] = $"Cannot add more than {book.Stock} copies to the cart.";
                    return RedirectToAction("Index", "Book");
                }
                existing.Quantity += quantity;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    AppUserId = userId,
                    BookId = bookId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Book added to cart!";
            return RedirectToAction("Index", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, int quantity)
        {
            var item = await _context.CartItems
                .Include(c => c.Book)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (item == null) return NotFound();

            if (quantity > item.Book!.Stock)
            {
                TempData["Error"] = $"Only {item.Book.Stock} copies available.";
                return RedirectToAction("Index");
            }

            if (quantity <= 0)
                _context.CartItems.Remove(item);
            else
                item.Quantity = quantity;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}



