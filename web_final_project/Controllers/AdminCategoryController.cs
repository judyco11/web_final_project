using Microsoft.AspNetCore.Mvc;
using OnlineBookStors.Data;
using OnlineBookStors.Models;

namespace OnlineBookStors.Controllers.Admin
{
    [AdminOnly]
    public class AdminCategoryController : Controller
    {
        private readonly AppDbContext _db;

        public AdminCategoryController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Categories.Add(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Categories.Update(model);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();

            _db.Categories.Remove(category);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}


