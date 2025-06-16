using Microsoft.AspNetCore.Mvc;

namespace BootcampDay1.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Name = "Taha Kerem";
            ViewBag.Surname = "Eriş";
            ViewBag.Job = "Yazılım Geliştirici";
            return View();
        }
    }
}