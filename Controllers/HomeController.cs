using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // If user is logged in, redirect to Dashboard (shopping page)
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            // If not logged in, show welcome page
            return View();
        }
    }
}
