// <copyright file="CipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Settings for the Reverse cipher.
    /// </summary>
    public class CipherSettings : ICipherSettings
    {
        private IEnumerable<byte> _key;
        private IEnumerable<byte> _iv;

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        public CipherSettings()
            : this(new KeyGenerator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        /// <param name="keyGenerator">The cipher's key generator.</param>
        public CipherSettings(IKeyGenerator keyGenerator)
        {
            KeyGenerator = keyGenerator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public CipherSettings(byte[] key, byte[] iv)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _iv = iv ?? throw new ArgumentNullException(nameof(iv));

            KeyGenerator = new KeyGenerator();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public virtual IEnumerable<byte> IV
        {
            get
            {
                if (_iv == null)
                {
                    _iv = KeyGenerator.DefaultKey;
                }

                return _iv;
            }

            protected set
            {
                _iv = value;
            }
        }

        /// <inheritdoc />
        public virtual IEnumerable<byte> Key
        {
            get
            {
                if (_key == null)
                {
                    _key = KeyGenerator.DefaultKey;
                }

                return _key;
            }

            protected set
            {
                _key = value;
            }
        }

        /// <inheritdoc />
        public IKeyGenerator KeyGenerator
        {
            get;
            protected set;
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