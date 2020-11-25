// <copyright file="ROT13Symmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// The ROT13 cipher.
    /// </summary>
    public class ROT13Symmetric : SymmetricAlgorithm
    {
        private readonly ROT13 _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ROT13Symmetric"/> class.
        /// </summary>
        public ROT13Symmetric()
        {
            Reset();
            _algorithm = new ROT13();
        }

        /// <inheritdoc />
        public override byte[] IV { get => Array.Empty<byte>(); set => _ = value; }

        /// <inheritdoc />
        public override byte[] Key { get => Array.Empty<byte>(); set => _ = value; }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            Key = rgbKey;
            IV = rgbIV ?? Array.Empty<byte>();
            return new ClassicalSymmetricTransform(_algorithm, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            Key = rgbKey;
            IV = rgbIV ?? Array.Empty<byte>();
            return new ClassicalSymmetricTransform(_algorithm, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override void GenerateIV()
        {
            IVValue = Array.Empty<byte>();
            IV = IVValue;
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = Array.Empty<byte>();
            Key = KeyValue;
        }

        /// <inheritdoc />
        public override string ToString() => _algorithm.CipherName;

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