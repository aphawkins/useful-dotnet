// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography;
using UsefulAPI.Models;

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
            if (request == null)
            {
                return UnprocessableEntity();
            }

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
            if (request == null)
            {
                return UnprocessableEntity();
            }

            Rot13 cipher = new();
            return new EncryptResponse()
            {
                Ciphertext = cipher.Encrypt(request.Plaintext),
            };
        }
    }
}
