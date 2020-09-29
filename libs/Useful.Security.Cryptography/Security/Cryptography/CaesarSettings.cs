// <copyright file="CaesarSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public sealed class CaesarSettings : ICaesarSettings
    {
        private const int DefaultShift = 1;

        /// <summary>
        /// The right shift.
        /// </summary>
        private int _rightShift;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSettings"/> class.
        /// </summary>
        public CaesarSettings()
            : this(DefaultShift)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSettings"/> class.
        /// </summary>
        /// <param name="rightShift">The right shift.</param>
        public CaesarSettings(int rightShift)
            : base()
        {
            if (rightShift is < 0 or > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(rightShift), "Value must be between 0 and 25.");
            }

            _rightShift = rightShift;
        }

        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public int RightShift
        {
            get => _rightShift;

            set
            {
                if (value is < 0 or > 25)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 25.");
                }

                _rightShift = value;
            }
        }
    }
}