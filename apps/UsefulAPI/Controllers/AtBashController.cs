// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography;
using UsefulAPI.Models;

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
            if (request == null)
            {
                return UnprocessableEntity();
            }

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
            ArgumentNullException.ThrowIfNull(request);

            Atbash cipher = new();
            return new EncryptResponse()
            {
                Ciphertext = cipher.Encrypt(request.Plaintext),
            };
        }
    }
}
