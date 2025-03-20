// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.AspNetCore.Mvc;
using Useful.Security.Cryptography;
using UsefulAPI.Models;

namespace UsefulAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class CaesarController : ControllerBase
{
    // POST api/v1/caesar/decrypt
    [HttpPost]
    public ActionResult<DecryptResponse> Decrypt(CaesarDecryptRequest request)
    {
        if (request == null)
        {
            return UnprocessableEntity();
        }

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
        if (request == null)
        {
            return UnprocessableEntity();
        }

        Caesar cipher = new(new CaesarSettings() { RightShift = request.RightShift });
        return new EncryptResponse()
        {
            Ciphertext = cipher.Encrypt(request.Plaintext),
        };
    }
}
