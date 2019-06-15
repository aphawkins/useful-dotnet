// <copyright file="CipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// Settings for the Reverse cipher.
    /// </summary>
    public class CipherSettings : ICipherSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        public CipherSettings()
            : this(Array.Empty<byte>(), Array.Empty<byte>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public CipherSettings(byte[] key, byte[] iv)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            IV = iv ?? throw new ArgumentNullException(nameof(iv));
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the character set.
        /// </summary>
        /// <value>The character set.</value>
        public string CharacterSet { get; protected set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <inheritdoc />
        public virtual IEnumerable<byte> IV
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public virtual IEnumerable<byte> Key
        {
            get;
            private set;
        }

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