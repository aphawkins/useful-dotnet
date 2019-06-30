﻿// <copyright file="HomeController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWeb.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Cryptography(CipherModel cipherViewModel)
        {
            if (cipherViewModel == null)
            {
                throw new ArgumentNullException(nameof(cipherViewModel));
            }

            if (Request.ContentLength.HasValue)
            {
                _cipherService.Repository.SetCurrentItem(x => x.CipherName == cipherViewModel.CurrentCipherName);

                if (Request.Form.ContainsKey("EncryptCommand"))
                {
                    cipherViewModel.Ciphertext = _cipherService.Encrypt(cipherViewModel.Plaintext);
                }
                else if (Request.Form.ContainsKey("DecryptCommand"))
                {
                    cipherViewModel.Plaintext = _cipherService.Decrypt(cipherViewModel.Ciphertext);
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