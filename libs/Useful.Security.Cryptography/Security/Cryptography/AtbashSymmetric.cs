// <copyright file="AtbashSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public class AtbashSymmetric : SymmetricAlgorithm
    {
        private readonly Atbash _algorithm;
        private readonly KeyGenerator _keyGen = new KeyGenerator();

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
            ICipher cipher = new Atbash();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            ICipher cipher = new Atbash();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override void GenerateIV()
        {
            IVValue = _keyGen.RandomIv();
            IV = IVValue;
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = _keyGen.RandomKey();
            Key = KeyValue;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _algorithm.CipherName;
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