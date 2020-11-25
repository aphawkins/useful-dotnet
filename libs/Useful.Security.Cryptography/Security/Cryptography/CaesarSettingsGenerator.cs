// <copyright file="CaesarSettingsGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;

    /// <summary>
    /// Caesar key generator.
    /// </summary>
    internal class CaesarSettingsGenerator
    {
        public static ICaesarSettings Generate()
        {
            byte[] randomNumber = new byte[1];
            using RNGCryptoServiceProvider rngCsp = new();
            rngCsp.GetBytes(randomNumber);
            int rightShift = randomNumber[0] % 26;
            return new CaesarSettings(rightShift);
        }
    }
}