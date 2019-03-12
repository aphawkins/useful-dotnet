// <copyright file="SymmetricCipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Accesses the Reverse Shift algorithm settings.
    /// </summary>
    public class SymmetricCipherSettings : CipherSettings, ISymmetricCipherSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricCipherSettings"/> class.
        /// </summary>
        public SymmetricCipherSettings()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricCipherSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public SymmetricCipherSettings(byte[] key, byte[] iv)
            : base()
        {
            Key = key;
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
                return KeyGenerator.DefaultKey;
            }

            private set
            {
            }
        }

        /// <inheritdoc />
        public ISymmetricKeyGenerator KeyGenerator { get; } = new SymmetricKeyGenerator();
    }
}