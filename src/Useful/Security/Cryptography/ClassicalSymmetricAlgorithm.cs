// <copyright file="ClassicalSymmetricAlgorithm.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Classical symmetric algorithm.
    /// </summary>
    public sealed class ClassicalSymmetricAlgorithm : SymmetricAlgorithm
    {
        private readonly ISymmetricCipher _cipher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassicalSymmetricAlgorithm"/> class.
        /// </summary>
        /// <param name="cipher">The classical cipher to use.</param>
        public ClassicalSymmetricAlgorithm(ISymmetricCipher cipher)
            : base()
        {
            _cipher = cipher ?? throw new ArgumentNullException(nameof(cipher));
            Reset();
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new ClassicalSymmetricTransform(_cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new ClassicalSymmetricTransform(_cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override void GenerateIV()
        {
            // IV is always empty.
            IVValue = _cipher.Settings.KeyGenerator.RandomIv();
        }

        /// <summary>
        /// Generates a random key to be used for the algorithm.
        /// </summary>
        public override void GenerateKey()
        {
            KeyValue = _cipher.Settings.KeyGenerator.RandomKey();
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