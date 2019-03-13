// <copyright file="ReverseSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Accesses the Reverse algorithm.
    /// </summary>
    public sealed class ReverseSymmetric : ClassicalSymmetricAlgorithm
    {
        private readonly IKeyGenerator _keyGen = new KeyGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseSymmetric"/> class.
        /// </summary>
        public ReverseSymmetric()
            : base()
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher(new CipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher(new CipherSettings(rgbKey, rgbIV));
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
            return "Reverse";
        }
    }
}