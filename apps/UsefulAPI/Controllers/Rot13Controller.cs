// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography;

namespace UsefulAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class Rot13Controller : ControllerBase
    {
        // POST api/v1/rot13/decrypt
        [HttpPost]
        public ActionResult<DecryptResponse> Decrypt([FromBody] DecryptRequest request)
        {
            Rot13 cipher = new();
            return new DecryptResponse()
            {
                Plaintext = cipher.Decrypt(request.Ciphertext),
            };
        }

        // POST api/v1/rot13/encrypt
        [HttpPost]
        public ActionResult<EncryptResponse> Encrypt([FromBody] EncryptRequest request)
        {
            Rot13 cipher = new();
            return new EncryptResponse()
            {
                Ciphertext = cipher.Encrypt(request.Plaintext),
            };
        }
    }
}