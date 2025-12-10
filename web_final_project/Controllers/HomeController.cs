using Microsoft.AspNetCore.Mvc;

namespace web_final_project.Controllers
{
    using web_final_project.Filters;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View(); // Create a simple admin dashboard view
        }

        public IActionResult CustomerDashboard()
        {
            return View(); // Create a simple customer dashboard view
        }

    }
}

