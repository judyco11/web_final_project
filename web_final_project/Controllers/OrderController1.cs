using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_final_project.Data;
using web_final_project.Models;


namespace web_final_project.Controllers
{
    using web_final_project.Filters;

    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.Include(o => o.User).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var o = await _context.Orders.FindAsync(id);
            if (o == null) return NotFound();

            o.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}