// <copyright file="CaesarKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Caesar key generator.
    /// </summary>
    public class CaesarKeyGenerator : ISymmetricKeyGenerator
    {
        /// <inheritdoc />
        public IEnumerable<byte> DefaultIv => Array.Empty<byte>();

        /// <inheritdoc />
        public IEnumerable<byte> DefaultKey => new List<byte>() { 1 };

        /// <inheritdoc />
        public byte[] RandomIv() => Array.Empty<byte>();

        /// <inheritdoc />
        public byte[] RandomKey()
        {
            byte[] randomNumber = new byte[1];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomNumber);
                return new byte[1] { (byte)((randomNumber[0] % 26) + 1) };
            }
        }
    }
}