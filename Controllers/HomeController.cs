using Microsoft.AspNetCore.Mvc;

namespace SRMS.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Privacy()
        {
            return View("Privacy");
        }
    }
}
