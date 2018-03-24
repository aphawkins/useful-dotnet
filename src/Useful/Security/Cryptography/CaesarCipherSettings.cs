// <copyright file="CaesarCipherSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public class CaesarCipherSettings : ICipherSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSettings"/> class.
        /// </summary>
        public CaesarCipherSettings()
        {
            RightShift = 0;
        }

        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public int RightShift
        {
            get;
            set;
        }
    }
}