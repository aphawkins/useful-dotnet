﻿// <copyright file="CaesarController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Useful.Security.Cryptography;

    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class CaesarController : ControllerBase
    {
        // POST api/v1/caesar/decrypt
        [HttpPost]
        public ActionResult<DecryptResponse> Decrypt(CaesarDecryptRequest request)
        {
            Caesar cipher = new(new CaesarSettings() { RightShift = request.RightShift });
            return new DecryptResponse()
            {
                Plaintext = cipher.Decrypt(request.Ciphertext),
            };
        }

        // POST api/v1/caesar/encrypt
        [HttpPost]
        public ActionResult<EncryptResponse> Encrypt(CaesarEncryptRequest request)
        {
            Caesar cipher = new(new CaesarSettings() { RightShift = request.RightShift });
            return new EncryptResponse()
            {
                Ciphertext = cipher.Encrypt(request.Plaintext),
            };
        }
    }
}