using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userNotifications = TempData.Keys
                .Where(k => k.StartsWith("Order_"))
                .Select(k => TempData[k])
                .ToList();

            ViewBag.Notifications = userNotifications;

            return View();
        }

    }
}
