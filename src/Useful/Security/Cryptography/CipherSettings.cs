// <copyright file="CipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Settings for the Reverse cipher.
    /// </summary>
    public class CipherSettings : ICipherSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        public CipherSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public CipherSettings(byte[] key, byte[] iv)
            : base()
        {
            Key = key;
            IV = iv;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the Initialization Vector.
        /// </summary>
        public virtual IEnumerable<byte> IV
        {
            get
            {
                return KeyGenerator.DefaultIv;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the encryption Key.
        /// </summary>
        /// <returns>The encryption key.</returns>
        public virtual IEnumerable<byte> Key
        {
            get
            {
                return KeyGenerator.DefaultKey;
            }

            set
            {
            }
        }

        /// <inheritdoc />
        public virtual IKeyGenerator KeyGenerator { get; } = new KeyGenerator();

        /// <summary>
        /// Used to raise the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}