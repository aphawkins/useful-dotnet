// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography.UI.Models;
using Useful.Security.Cryptography.UI.Services;

namespace UsefulWeb.Controllers
{
    public class CryptographyController(CipherService cipherService) : Controller
    {
        private readonly CipherService _cipherService = cipherService;

        public IActionResult Index() => RedirectToAction("Cryptography");

        public IActionResult Cryptography() => View();

        public IActionResult AtBash(CipherModel cipherModel)
        {
            ArgumentNullException.ThrowIfNull(cipherModel);

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
            ArgumentNullException.ThrowIfNull(cipherModel);

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