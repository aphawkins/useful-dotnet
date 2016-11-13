namespace UsefulWeb.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Useful.UI.ViewModels;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Cryptography");
        }

        public IActionResult Cryptography(CipherViewModel model)
        {
            model.Encrypt();

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}