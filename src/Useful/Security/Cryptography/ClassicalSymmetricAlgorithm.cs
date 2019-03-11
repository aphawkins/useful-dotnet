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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TU"></typeparam>
    public sealed class ClassicalSymmetricAlgorithm<T, TU> : SymmetricAlgorithm
        where T : ICipher
        where TU : ISymmetricCipherSettings
    {
        private readonly ISymmetricKeyGenerator _keyGen = new SymmetricKeyGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassicalSymmetricAlgorithm{T}"/> class.
        /// </summary>
        public ClassicalSymmetricAlgorithm()
            : base()
        {
            Reset();
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new SymmetricTransform<T, TU>(rgbKey, rgbIV, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new SymmetricTransform<T, TU>(rgbKey, rgbIV, CipherTransformMode.Encrypt);
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