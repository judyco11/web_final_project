using Microsoft.AspNetCore.Mvc;
using web_final_project.Data;
using web_final_project.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace web_final_project.Controllers
{
    using web_final_project.Filters;

    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var books = _context.Books.ToList(); // assuming _context is your DbContext
            return View(books);
        }
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(AppUser user)
        {
            if (!ModelState.IsValid) return View(user);

            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("", "Username already exists");
                return View(user);
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            return user.IsAdmin ? RedirectToAction("AdminDashboard", "Home") : RedirectToAction("CustomerDashboard", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Edit()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null) return RedirectToAction("Login");

            return View(user);
        }


        [HttpPost]
        public IActionResult Edit(AppUser updatedUser)
        {
            if (!ModelState.IsValid) return View(updatedUser);

            var user = _context.Users.Find(updatedUser.UserId);
            if (user == null) return NotFound();

            user.FullName = updatedUser.FullName;
            user.Address = updatedUser.Address;
            user.City = updatedUser.City;
            user.Phone = updatedUser.Phone;
            user.Email = updatedUser.Email;

            _context.SaveChanges();
            return RedirectToAction("CustomerDashboard", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}


