// <copyright file="CaesarCipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

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
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public CaesarCipherSettings(byte[] key, byte[] iv)
            : this(GetRightShift(key))
        {
            IV = iv;
        }

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
        /// Gets the Initialization Vector.
        /// </summary>
        public override IEnumerable<byte> IV
        {
            get
            {
                return KeyGenerator.DefaultIv;
            }
        }

        /// <summary>
        /// Gets or sets the encryption Key.
        /// </summary>
        /// <returns>The encryption key.</returns>
        public override IEnumerable<byte> Key
        {
            get
            {
                return new Collection<byte>(Encoding.Unicode.GetBytes($"{RightShift}"));
            }

            set
            {
                RightShift = int.Parse(Encoding.Unicode.GetString(new List<byte>(value).ToArray()), CultureInfo.InvariantCulture);

                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override IKeyGenerator KeyGenerator { get; } = new CaesarKeyGenerator();

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

        private static int GetRightShift(byte[] key)
        {
            return int.Parse(Encoding.Unicode.GetString(key), CultureInfo.InvariantCulture);
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