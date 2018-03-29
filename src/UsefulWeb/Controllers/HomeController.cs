// <copyright file="HomeController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

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
            if (Request.ContentLength.HasValue)
            {
                if (Request.Form.ContainsKey("EncryptCommand"))
                {
                    model.Encrypt();
                }
                else if (Request.Form.ContainsKey("DecryptCommand"))
                {
                    model.Decrypt();
                }
            }

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}