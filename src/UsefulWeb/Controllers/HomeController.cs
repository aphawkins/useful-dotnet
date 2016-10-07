namespace UsefulWeb.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Useful.UI.ViewModels;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cryptography(CipherViewModel model)
        {
            model.Encrypt();

            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}