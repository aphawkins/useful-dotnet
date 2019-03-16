// <copyright file="ClassicalSymmetricAlgorithm.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using Useful.Interfaces.Security.Cryptography;

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
        /// <param name="keyGenerator">For creating default and random keys.</param>
        public ClassicalSymmetricAlgorithm(string cipherName, ICipherSettings settings, IKeyGenerator keyGenerator)
            : base()
        {
            CipherName = cipherName;
            Settings = settings;
            KeyGenerator = keyGenerator;
            Reset();
        }

        /// <inheritdoc />
        public virtual IKeyGenerator KeyGenerator
        {
            get;
            internal set;
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
            set;
        }

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
            // IV is always empty.
            IVValue = KeyGenerator.RandomIv();
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = KeyGenerator.RandomKey();
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
            LegalKeySizesValue[0] = new KeySizes(16, 16, 16);

            GenerateKey();
            GenerateIV();
        }
    }
}