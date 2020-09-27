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
        public string Decrpyt([FromBody] string ciphertext)
        {
            using ROT13 cipher = new ROT13();
            return cipher.Decrypt(ciphertext);
        }

        // POST api/v1/rot13/encrypt
        [HttpPost]
        public string Encrpyt([FromBody] string plaintext)
        {
            using ROT13 cipher = new ROT13();
            return cipher.Encrypt(plaintext);
        }
    }
}