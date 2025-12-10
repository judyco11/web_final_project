using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_final_project.Data;
using web_final_project.Models;

namespace web_final_project.Controllers
{
    using web_final_project.Filters;

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Categories.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryId) return NotFound();

            if (!ModelState.IsValid) return View(category);

            _context.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c != null)
            {
                _context.Categories.Remove(c);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

}
