using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;

namespace OnlineBookStore.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminStatsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminStatsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Total revenue
            var totalRevenue = await _context.OrderItems
    .Where(i => i.Order.Status == "Confirmed" || i.Order.Status == "Shipped")
    .SumAsync(i => i.UnitPrice * i.Quantity);


            //Total sales count
            var totalSales = await _context.OrderItems
    .Where(i => i.Order.Status == "Confirmed" || i.Order.Status == "Shipped")
    .SumAsync(i => i.Quantity);


            // Top-selling books
            var topBooks = await _context.OrderItems
    .Include(i => i.Book)
    .Where(i => i.Order.Status == "Confirmed" || i.Order.Status == "Shipped")
    .GroupBy(i => i.Book.Title)
    .Select(g => new
    {
        Title = g.Key,
        Quantity = g.Sum(i => i.Quantity),
        Revenue = g.Sum(i => i.UnitPrice * i.Quantity)
    })
    .OrderByDescending(g => g.Quantity)
    .Take(5)
    .ToListAsync();


            // Most popular categories
            var topCategories = await _context.OrderItems
    .Include(i => i.Book)
    .ThenInclude(b => b.Category)
    .Where(i => i.Order.Status == "Confirmed" || i.Order.Status == "Shipped")
    .GroupBy(i => i.Book.Category.Name)
    .Select(g => new
    {
        Category = g.Key,
        Quantity = g.Sum(i => i.Quantity)
    })
    .OrderByDescending(g => g.Quantity)
    .Take(5)
    .ToListAsync();


            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalSales = totalSales;
            ViewBag.TopBooks = topBooks;
            ViewBag.TopCategories = topCategories;

            return View();
        }
    }
}
