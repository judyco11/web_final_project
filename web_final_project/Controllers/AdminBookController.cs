using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBookStore.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminBookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminBookController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 📌 LIST ALL BOOKS
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                                      .Include(b => b.Category)
                                      .ToListAsync();
            return View(books);
        }

        // 📌 CREATE (GET)
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        // 📌 CREATE (POST) + IMAGE UPLOAD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile coverFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(book);
            }

            if (coverFile != null && coverFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images/books");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid() + Path.GetExtension(coverFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await coverFile.CopyToAsync(stream);
                }

                // ✅ Store relative URL
                book.CoverImageUrl = "/images/books/" + fileName;
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // 📌 EDIT (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(book);
        }

        // 📌 EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book updatedBook, IFormFile coverFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(updatedBook);
            }

            // Load the tracked entity from the database
            var dbBook = await _context.Books.FindAsync(updatedBook.Id);
            if (dbBook == null) return NotFound();

            // Update fields
            dbBook.Title = updatedBook.Title;
            dbBook.Author = updatedBook.Author;
            dbBook.Price = updatedBook.Price;
            dbBook.Stock = updatedBook.Stock;
            dbBook.Edition = updatedBook.Edition;
            dbBook.CategoryId = updatedBook.CategoryId;

            // Handle cover file upload
            if (coverFile != null && coverFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "images/books");
                Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid() + Path.GetExtension(coverFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await coverFile.CopyToAsync(stream);
                }

                dbBook.CoverImageUrl = "/images/books/" + fileName;
            }
            // else keep existing image automatically

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // 📌 DELETE (GET)
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books
                                     .Include(b => b.Category)
                                     .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        // 📌 DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
