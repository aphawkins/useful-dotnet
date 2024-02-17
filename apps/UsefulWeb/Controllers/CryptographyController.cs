// <copyright file="CryptographyController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography.UI.Models;
using Useful.Security.Cryptography.UI.Services;

namespace UsefulWeb.Controllers
{
    public class CryptographyController : Controller
    {
        private readonly CipherService _cipherService;

        public CryptographyController(CipherService cipherService) => _cipherService = cipherService;

        public IActionResult Index() => RedirectToAction("Cryptography");

        public IActionResult Cryptography() => View();

        public IActionResult AtBash(CipherModel cipherModel)
        {
            if (cipherModel == null)
            {
                throw new ArgumentNullException(nameof(cipherModel));
            }

            if (Request.ContentLength.HasValue)
            {
                _cipherService.Repository.SetCurrentItem(x => x.CipherName == "Atbash");

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

        public IActionResult ROT13(CipherModel cipherModel)
        {
            if (cipherModel == null)
            {
                throw new ArgumentNullException(nameof(cipherModel));
            }

            if (Request.ContentLength.HasValue)
            {
                _cipherService.Repository.SetCurrentItem(x => x.CipherName == "ROT13");

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

        public IActionResult Error() => View();
    }
}