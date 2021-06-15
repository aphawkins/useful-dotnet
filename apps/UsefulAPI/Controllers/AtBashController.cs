// <copyright file="AtBashController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Useful.Security.Cryptography;

    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AtBashController : ControllerBase
    {
        // POST api/v1/atbash/decrypt
        [HttpPost]
        public ActionResult<DecryptResponse> Decrypt([FromBody] string ciphertext)
        {
            Atbash cipher = new();
            return new DecryptResponse()
            {
                Plaintext = cipher.Decrypt(ciphertext),
            };
        }

        // POST api/v1/atbash/encrypt
        [HttpPost]
        public ActionResult<EncryptResponse> Encrypt([FromBody] string plaintext)
        {
            Atbash cipher = new();
            return new EncryptResponse()
            {
                Ciphertext = cipher.Encrypt(plaintext),
            };
        }
    }
}