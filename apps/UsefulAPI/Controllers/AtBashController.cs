// <copyright file="AtBashController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography;

namespace UsefulAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AtBashController : ControllerBase
    {
        // POST api/v1/atbash/decrypt
        [HttpPost]
        public ActionResult<DecryptResponse> Decrypt(DecryptRequest request)
        {
            Atbash cipher = new();
            return new DecryptResponse()
            {
                Plaintext = cipher.Decrypt(request.Ciphertext),
            };
        }

        // POST api/v1/atbash/encrypt
        [HttpPost]
        public ActionResult<EncryptResponse> Encrypt(EncryptRequest request)
        {
            Atbash cipher = new();
            return new EncryptResponse()
            {
                Ciphertext = cipher.Encrypt(request.Plaintext),
            };
        }
    }
}