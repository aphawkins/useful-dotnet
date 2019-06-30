// <copyright file="ClassicalSymmetricAlgorithm.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// Classical symmetric algorithm.
    /// </summary>
    public abstract class ClassicalSymmetricAlgorithm : SymmetricAlgorithm, ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassicalSymmetricAlgorithm"/> class.
        /// </summary>
        /// <param name="cipherName">The cipher name.</param>
        /// <param name="settings">The cipher settings.</param>
        public ClassicalSymmetricAlgorithm(string cipherName, ICipherSettings settings)
            : base()
        {
            CipherName = cipherName;
            Settings = settings;
            Reset();
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get => Settings.IV.ToArray();
            set => _ = value;
        }

        /// <inheritdoc />
        public override byte[] Key
        {
            get => Settings.Key.ToArray();
            set => _ = value;
        }

        /// <inheritdoc />
        public virtual string CipherName
        {
            get;
            internal set;
        }

        /// <inheritdoc />
        public virtual ICipherSettings Settings
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the key generator.
        /// </summary>
        protected IKeyGenerator KeyGenerator { get; set; } = new KeyGenerator();

        /// <inheritdoc />
        public abstract string Decrypt(string ciphertext);

        /// <inheritdoc />
        public abstract string Encrypt(string plaintext);

        /// <summary>
        /// The name of the Cipher.
        /// </summary>
        /// <returns>Name of the Cipher.</returns>
        public override string ToString() => CipherName;

        /// <inheritdoc />
        public override void GenerateIV()
        {
            IVValue = KeyGenerator.RandomIv();
            IV = IVValue;
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = KeyGenerator.RandomKey();
            Key = KeyValue;
        }

        private void Reset()
        {
            ModeValue = CipherMode.ECB;
            PaddingValue = PaddingMode.None;
            KeySizeValue = 16;
            BlockSizeValue = 16;
            FeedbackSizeValue = 16;
            LegalBlockSizesValue = new KeySizes[1];
            LegalBlockSizesValue[0] = new KeySizes(16, 16, 16);
            LegalKeySizesValue = new KeySizes[1];
            LegalKeySizesValue[0] = new KeySizes(0, int.MaxValue, 16);

            KeyValue = Array.Empty<byte>();
            IVValue = Array.Empty<byte>();
        }
    }
}