﻿// <copyright file="AtbashSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public sealed class AtbashSymmetric : SymmetricAlgorithm
    {
        private readonly Atbash _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtbashSymmetric"/> class.
        /// </summary>
        public AtbashSymmetric()
        {
            Reset();
            _algorithm = new Atbash();
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
            return new ClassicalSymmetricTransform(_algorithm, CipherTransformMode.Encrypt);
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