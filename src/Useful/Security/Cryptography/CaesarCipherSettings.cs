// <copyright file="CaesarCipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public class CaesarCipherSettings : CipherSettings
    {
        /// <summary>
        /// The right shift.
        /// </summary>
        private int _rightShift;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSettings"/> class.
        /// </summary>
        /// <param name="rightShift">The right shift.</param>
        public CaesarCipherSettings(int rightShift)
        {
            ValidateRightShift(rightShift);

            _rightShift = rightShift;
        }

        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public virtual int RightShift
        {
            get
            {
                return _rightShift;
            }

            set
            {
                ValidateRightShift(value);

                _rightShift = value;

                NotifyPropertyChanged();
            }
        }

        private static void ValidateRightShift(int value)
        {
            if (value < 0 || value > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 25.");
            }
        }
    }
}