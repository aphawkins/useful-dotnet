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

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public sealed class CaesarCipherSettings : CipherSettings
    {
        /// <summary>
        /// The right shift.
        /// </summary>
        private int _rightShift;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSettings"/> class.
        /// </summary>
        public CaesarCipherSettings()
        {
            KeyGenerator = new CaesarKeyGenerator();
            Key = KeyGenerator.DefaultKey;
            IV = KeyGenerator.DefaultIv;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        public CaesarCipherSettings(byte[] key)
            : this(GetRightShift(key))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSettings"/> class.
        /// </summary>
        /// <param name="rightShift">The right shift.</param>
        public CaesarCipherSettings(int rightShift)
            : this()
        {
            ValidateRightShift(rightShift);

            KeyGenerator = new CaesarKeyGenerator();
            _rightShift = rightShift;
        }

        /// <inheritdoc />
        public override IEnumerable<byte> IV
        {
            get;
            protected set;
        }

        /// <inheritdoc />
        public override IEnumerable<byte> Key
        {
            get
            {
                return new Collection<byte>(Encoding.Unicode.GetBytes($"{RightShift}"));
            }

            protected set
            {
                RightShift = int.Parse(Encoding.Unicode.GetString(new List<byte>(value).ToArray()), CultureInfo.InvariantCulture);

                NotifyPropertyChanged();
            }
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