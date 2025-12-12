using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OnlineBookStors.Attributes;
using OnlineBookStors.Data;
using OnlineBookStors.Models;

[AdminOnly]
public class AdminBookController : Controller
{
    private readonly AppDbContext _context;
    public AdminBookController(AppDbContext context) => _context = context;

    public IActionResult Index()
    {
        var books = _context.Books.Include(b => b.Category).ToList();
        return View(books);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Book model)
    {
        _context.Books.Add(model);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View(_context.Books.Find(id));
    }

    [HttpPost]
    public IActionResult Edit(Book b)
    {
        _context.Books.Update(b);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var b = _context.Books.Find(id);
        if (b != null) _context.Books.Remove(b);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
