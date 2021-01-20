// <copyright file="CaesarSettingsGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;

    /// <summary>
    /// Caesar key generator.
    /// </summary>
    internal sealed class CaesarSettingsGenerator
    {
        public static CaesarSettings Generate()
        {
            byte[] randomNumber = new byte[1];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            int rightShift = randomNumber[0] % 26;
            return new CaesarSettings() { RightShift = rightShift };
        }
    }
}