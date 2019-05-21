// <copyright file="EnigmaKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// Caesar key generator.
    /// </summary>
    internal class EnigmaKeyGenerator : IKeyGenerator
    {
        /// <inheritdoc />
        public byte[] RandomIv()
        {
            return Encoding.Unicode.GetBytes("A A A");
        }

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