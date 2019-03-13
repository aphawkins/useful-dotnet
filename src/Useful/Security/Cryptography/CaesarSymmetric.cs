// <copyright file="CaesarSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Accesses the Caesar Shift algorithm.
    /// </summary>
    public sealed class CaesarSymmetric : ClassicalSymmetricAlgorithm
    {
        private readonly IKeyGenerator _keyGen = new CaesarKeyGenerator();

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new CaesarCipher(new CaesarCipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new CaesarCipher(new CaesarCipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override void GenerateIV()
        {
            // IV is always empty.
            IVValue = _keyGen.RandomIv();
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = _keyGen.RandomKey();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Caesar";
        }
    }
}