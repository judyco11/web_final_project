using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System.Security.Claims;

namespace OnlineBookStore.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Add(int bookId, int rating, string comment)
        {
            // 1️⃣ Get the logged-in user's ID
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid user ID.");
            }

            // 2️⃣ Check if the user exists
            var user = _context.AppUsers.Find(userId);
            if (user == null)
            {
                return BadRequest("User does not exist in the database.");
            }

            // 3️⃣ Check if the book exists
            var book = _context.Books.Find(bookId);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            // 4️⃣ Create and add the review
            var review = new Reviews
            {
                BookId = bookId,
                AppUserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Details", "Book", new { id = bookId });
        }
    }
}
