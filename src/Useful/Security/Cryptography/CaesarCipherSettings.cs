// <copyright file="CaesarCipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public class CaesarCipherSettings : ICipherSettings
    {
        private int _rightShift;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSettings"/> class.
        /// </summary>
        public CaesarCipherSettings()
        {
            _rightShift = 0;
        }

        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public int RightShift
        {
            get
            {
                return _rightShift;
            }

            set
            {
                if (value < 0 || value > 26)
                {
                    throw new ArgumentOutOfRangeException(nameof(RightShift), "Value must be between 0 and 26.");
                }

                _rightShift = value;
            }
        }
    }
}