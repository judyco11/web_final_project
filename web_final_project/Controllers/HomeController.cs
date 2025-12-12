using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStors.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
