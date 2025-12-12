using Microsoft.AspNetCore.Mvc;
using OnlineBookStors.Attributes;
using OnlineBookStors.Data;

[AdminOnly]
public class AdminUserController : Controller
{
    private readonly AppDbContext _context;

    public AdminUserController(AppDbContext context) => _context = context;

    public IActionResult Index()
    {
        return View(_context.Users.ToList());
    }

    public IActionResult MakeAdmin(int id)
    {
        var user = _context.Users.Find(id);
        if (user != null)
        {
            user.Role = "Admin";
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}

