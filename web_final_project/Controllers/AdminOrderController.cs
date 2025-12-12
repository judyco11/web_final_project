using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OnlineBookStors.Attributes;
using OnlineBookStors.Data;

[AdminOnly]
public class AdminOrderController : Controller
{
    private readonly AppDbContext _context;

    public AdminOrderController(AppDbContext context) => _context = context;

    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .ThenInclude(i => i.Book)
            .ToList();

        return View(orders);
    }

    public IActionResult Confirm(int id)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            order.Status = "Confirmed";
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Ship(int id)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            order.Status = "Shipped";
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Cancel(int id)
    {
        var order = _context.Orders.Find(id);
        if (order != null)
        {
            order.Status = "Cancelled";
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}

