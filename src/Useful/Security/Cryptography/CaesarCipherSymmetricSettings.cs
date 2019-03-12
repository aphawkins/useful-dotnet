// <copyright file="CaesarCipherSymmetricSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Accesses the Caesar Shift algorithm settings.
    /// </summary>
    public class CaesarCipherSymmetricSettings : CaesarCipherSettings, ISymmetricCipherSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSymmetricSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public CaesarCipherSymmetricSettings(byte[] key, byte[] iv)
            : base(GetRightShift(key))
        {
            IV = iv;
        }

        /// <summary>
        /// Gets the Initialization Vector.
        /// </summary>
        public IEnumerable<byte> IV
        {
            get
            {
                return KeyGenerator.DefaultIv;
            }

            private set
            {
            }
        }

        /// <summary>
        /// Gets the encryption Key.
        /// </summary>
        /// <returns>The encryption key.</returns>
        public IEnumerable<byte> Key
        {
            get
            {
                return new Collection<byte>(Encoding.Unicode.GetBytes($"{RightShift}"));
            }

            private set
            {
                RightShift = int.Parse(Encoding.Unicode.GetString(new List<byte>(value).ToArray()), CultureInfo.InvariantCulture);

                NotifyPropertyChanged();
            }
        }

        /// <inheritdoc />
        public ISymmetricKeyGenerator KeyGenerator { get; } = new CaesarKeyGenerator();

        private static int GetRightShift(byte[] key)
        {
            return int.Parse(Encoding.Unicode.GetString(key), CultureInfo.InvariantCulture);
        }
    }
}