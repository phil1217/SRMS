using Microsoft.AspNetCore.Mvc;
using SRMS.Models;
using SRMS.Utils;
using System.Diagnostics;
using System.Security.Claims;

namespace SRMS.Controllers
{
    public class UserController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StudentView()
        {
            return RedirectToAction("Index","Student");
        }

        public IActionResult FacultyView()
        {
            return RedirectToAction("Index", "Faculty");
        }

        public IActionResult AdminView()
        {
            return RedirectToAction("Index", "Admin");
        }

    }
}
