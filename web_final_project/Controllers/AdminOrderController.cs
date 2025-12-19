using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminOrderController : Controller
    {
        private readonly AppDbContext _context;

        public AdminOrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminOrder
        public async Task<IActionResult> Index(string? status)
        {
            var query = _context.Orders
                .Include(o => o.AppUser)
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.Status == status);

            ViewBag.Status = status;

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: AdminOrder/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.AppUser)
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: AdminOrder/Confirm
        [HttpPost]
       
        public async Task<IActionResult> Confirm(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            if (order.Status != "Pending")
            {
                TempData["Error"] = "Order cannot be confirmed.";
                return RedirectToAction(nameof(Index));
            }

            // Check stock
            foreach (var item in order.Items)
            {
                if (item.Book!.Stock < item.Quantity)
                {
                    TempData["Error"] = $"Not enough stock for {item.Book.Title}";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Reduce stock
            foreach (var item in order.Items)
            {
                item.Book!.Stock -= item.Quantity;
            }

            order.Status = "Confirmed";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Order confirmed and stock updated.";
            return RedirectToAction(nameof(Index));
        }


        // POST: AdminOrder/Ship
        [HttpPost]
        public async Task<IActionResult> Ship(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            if (order.Status == "Confirmed")
            {
                order.Status = "Shipped";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: AdminOrder/Cancel
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            if (order.Status == "Pending" || order.Status == "Confirmed")
            {
                order.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

