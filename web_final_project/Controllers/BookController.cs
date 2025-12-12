using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStors.Data;

namespace OnlineBookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        public BookController(AppDbContext context) => _context = context;

        public IActionResult Index(string search, int? categoryId)
        {
            var books = _context.Books
                .Include(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                books = books.Where(b => b.Title.Contains(search) || b.Author.Contains(search));

            if (categoryId.HasValue)
                books = books.Where(b => b.CategoryId == categoryId.Value);

            return View(books.ToList());
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);

            if (book == null) return NotFound();
            return View(book);
        }
    }
}




