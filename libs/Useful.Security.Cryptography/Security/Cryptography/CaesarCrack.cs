// <copyright file="CaesarCrack.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The Caesar cipher cracker.
    /// </summary>
    public static class CaesarCrack
    {
        /// <summary>
        /// Calculates the optimal settings.
        /// </summary>
        /// <param name="ciphertext">The text to crack.</param>
        /// <returns>The best guess crack.</returns>
        public static (int BestShift, IDictionary<int, string> AllDecryptions) Crack(string ciphertext)
        {
            Dictionary<int, string> shifts = new();
            CaesarSettings settings = new();
            Caesar cipher = new(settings);
            for (int i = 0; i < 26; i++)
            {
                cipher.Settings.RightShift = i;
                shifts.Add(i, cipher.Decrypt(ciphertext));
            }

            return (0, shifts);
        }
    }
}