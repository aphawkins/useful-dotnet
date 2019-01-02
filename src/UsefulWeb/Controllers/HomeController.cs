// <copyright file="HomeController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWeb.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Useful.UI.ViewModels;

    public class HomeController : Controller
    {
        private readonly CipherService cipherService;

        public HomeController(CipherService cipherService)
        {
            this.cipherService = cipherService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Cryptography");
        }

        public IActionResult Cryptography(CipherModel cipherViewModel)
        {
            if (cipherViewModel == null)
            {
                throw new ArgumentNullException(nameof(cipherViewModel));
            }

            if (Request.ContentLength.HasValue)
            {
                cipherService.Repository.SetCurrentItem(x => x.CipherName == cipherViewModel.CurrentCipherName);

                if (Request.Form.ContainsKey("EncryptCommand"))
                {
                    cipherViewModel.Ciphertext = cipherService.Encrypt(cipherViewModel.Plaintext);
                }
                else if (Request.Form.ContainsKey("DecryptCommand"))
                {
                    cipherViewModel.Plaintext = cipherService.Decrypt(cipherViewModel.Ciphertext);
                }
            }

            return View(cipherViewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}