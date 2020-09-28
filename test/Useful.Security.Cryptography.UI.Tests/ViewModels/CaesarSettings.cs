// <copyright file="CaesarSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public sealed class CaesarSettings : CipherSettings
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
        /// <param name="key">The encryption Key.</param>
        public CaesarSettings(byte[] key)
            : this(GetRightShift(key))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSettings"/> class.
        /// </summary>
        /// <param name="rightShift">The right shift.</param>
        public CaesarSettings(int rightShift)
            : base()
        {
            if (rightShift < 0 || rightShift > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(rightShift), "Value must be between 0 and 25.");
            }

            _rightShift = rightShift;
        }

        /// <inheritdoc />
        public override IEnumerable<byte> Key => new List<byte>(Encoding.Unicode.GetBytes($"{RightShift}"));

        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public int RightShift
        {
            get => _rightShift;

            set
            {
                if (value < 0 || value > 25)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 25.");
                }

                _rightShift = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Key));
            }
        }

        private static int GetRightShift(byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!int.TryParse(Encoding.Unicode.GetString(key), out int rightShift))
            {
                throw new ArgumentException("Value must be a number.", nameof(key));
            }

            if (rightShift < 0 || rightShift > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(key), "Value must be between 0 and 25.");
            }

            return rightShift;
        }
    }
}