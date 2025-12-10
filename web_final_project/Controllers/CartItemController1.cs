using Microsoft.AspNetCore.Mvc;
using web_final_project.Data;
using web_final_project.Models;

namespace web_final_project.Controllers
{
    using web_final_project.Filters;

    public class CartItemController : Controller
    {
        private readonly AppDbContext _context;
        public CartItemController(AppDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> Add(int cartId, int bookId)
        {
            var item = new CartItem { CartId = cartId, BookId = bookId, Quantity = 1 };
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();

            return Redirect($"/Cart?userId={cartId}");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
