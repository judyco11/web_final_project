using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using web_final_project.Migrations;
using web_final_project.Models;
namespace web_final_project.Controllers
{
    public class Book : Controller
    {
        private readonly ApplicationDbContext _context;

        public Book(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Book
        public IActionResult Index()
        {
            var books = _context.Books.Include(b => b.Category).ToList();
            return View(books);
        }

        // GET: /Book/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", book.CategoryId);
            return View(book);
        }

        // Optional: Edit/Delete actions can be added similarly
    }
}
