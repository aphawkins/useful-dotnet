// <copyright file="CaesarSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Accesses the Caesar Shift algorithm.
    /// </summary>
    public sealed class CaesarSymmetric : SymmetricAlgorithm
    {
        private readonly ISymmetricKeyGenerator _keyGen = new CaesarKeyGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSymmetric"/> class.
        /// </summary>
        public CaesarSymmetric()
            : base()
        {
            Reset();
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new CaesarTransform(rgbKey, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new CaesarTransform(rgbKey, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override void GenerateIV()
        {
            // IV is always empty.
            IVValue = _keyGen.RandomIv();
        }

        /// <summary>
        /// Generates a random key to be used for the algorithm.
        /// </summary>
        public override void GenerateKey()
        {
            KeyValue = _keyGen.RandomKey();
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