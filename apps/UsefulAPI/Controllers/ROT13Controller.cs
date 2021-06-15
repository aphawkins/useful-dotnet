// <copyright file="ROT13Controller.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Useful.Security.Cryptography;

    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class ROT13Controller : ControllerBase
    {
        // POST api/v1/rot13/decrypt
        [HttpPost]
        public ActionResult<DecryptResponse> Decrypt([FromBody] string ciphertext)
        {
            ROT13 cipher = new();
            return new DecryptResponse()
            {
                Plaintext = cipher.Decrypt(ciphertext),
            };
        }

        // POST api/v1/rot13/encrypt
        [HttpPost]
        public ActionResult<EncryptResponse> Encrypt([FromBody] string plaintext)
        {
            ROT13 cipher = new();
            return new EncryptResponse()
            {
                Ciphertext = cipher.Encrypt(plaintext),
            };
        }
    }
}