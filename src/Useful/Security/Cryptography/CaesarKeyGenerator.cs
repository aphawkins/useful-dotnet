﻿// <copyright file="CaesarKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Caesar key generator.
    /// </summary>
    public class CaesarKeyGenerator : IKeyGenerator
    {
        /// <inheritdoc />
        public byte[] RandomIv() => Array.Empty<byte>();

        /// <inheritdoc />
        public byte[] RandomKey()
        {
            byte[] randomNumber = new byte[1];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomNumber);
                return Encoding.Unicode.GetBytes($"{randomNumber[0] % 26}");
            }
        }
    }
}