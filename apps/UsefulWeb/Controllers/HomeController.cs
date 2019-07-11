// <copyright file="HomeController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWeb.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Models;
    using Useful.Security.Cryptography.UI.Services;

    public class HomeController : Controller
    {
        private readonly CipherService _cipherService;

        public HomeController(CipherService cipherService)
        {
            _cipherService = cipherService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Cryptography");
        }

        public IActionResult Cryptography(CipherModel cipherModel)
        {
            if (cipherModel == null)
            {
                throw new ArgumentNullException(nameof(cipherModel));
            }

            if (Request.ContentLength.HasValue)
            {
                _cipherService.Repository.SetCurrentItem(x => x.CipherName == cipherModel.CurrentCipherName);

                if (Request.Form.ContainsKey("EncryptCommand"))
                {
                    cipherModel.Ciphertext = _cipherService.Encrypt(cipherModel.Plaintext);
                }
                else if (Request.Form.ContainsKey("DecryptCommand"))
                {
                    cipherModel.Plaintext = _cipherService.Decrypt(cipherModel.Ciphertext);
                }
            }

            return View(cipherModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}