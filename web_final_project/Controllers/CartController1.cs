using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_final_project.Data;
using web_final_project.Models;

namespace web_final_project.Controllers
{
    using web_final_project.Filters;

    public class CartController : Controller
    {

        private readonly AppDbContext _context;
        public CartController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index(int userId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)!
                .ThenInclude(ci => ci.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return NotFound();
            return View(cart);
        }
    }
}
