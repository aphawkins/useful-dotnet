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
        public string Decrpyt([FromBody] string ciphertext)
        {
            using Atbash cipher = new();
            return cipher.Decrypt(ciphertext);
        }

        // POST api/v1/atbash/encrypt
        [HttpPost]
        public ITest Encrpyt([FromBody] string plaintext)
        {
            using Atbash cipher = new();
            TestResponse test = new()
            {
                Ciphertext = cipher.Encrypt(plaintext),
            };

            return test;
        }
    }
}