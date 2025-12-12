using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStors.Data;
using OnlineBookStors.Models;
using System.Security.Claims;

namespace OnlineBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context) => _context = context;

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ViewBag.Error = "Username already exists";
                return View(model);
            }

            model.Role = "Customer";
            _context.Users.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            await HttpContext.SignInAsync("CustomAuth",
                new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth")));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CustomAuth");
            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            int id = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            return View(_context.Users.Find(id));
        }

        public IActionResult EditProfile()
        {
            int id = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            return View(_context.Users.Find(id));
        }

        [HttpPost]
        public IActionResult EditProfile(User u)
        {
            var user = _context.Users.Find(u.Id);
            if (user == null) return NotFound();

            user.Email = u.Email;
            user.Phone = u.Phone;
            user.Address = u.Address;

            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
    }
}




