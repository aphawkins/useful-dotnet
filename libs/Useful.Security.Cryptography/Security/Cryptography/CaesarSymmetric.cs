﻿// <copyright file="CaesarSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The Caesar cipher.
    /// </summary>
    public class CaesarSymmetric : SymmetricAlgorithm
    {
        private readonly Caesar _algorithm;
        private readonly CaesarKeyGenerator _keyGen = new CaesarKeyGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSymmetric"/> class.
        /// </summary>
        public CaesarSymmetric()
        {
            Reset();
            _algorithm = new Caesar(new CaesarSettings());
            _keyGen = new CaesarKeyGenerator();
        }

        /// <inheritdoc />
        public override byte[] Key
        {
            get => Encoding.Unicode.GetBytes($"{_algorithm.Settings.RightShift}");

            set
            {
                _algorithm.Settings = GetSettings(value);
                base.Key = value;
            }
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get => Array.Empty<byte>();
            set => _ = value;
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            ICipher cipher = new Caesar(GetSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            ICipher cipher = new Caesar(GetSettings(rgbKey));
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
        public override string ToString() => _algorithm.CipherName;

        private static ICaesarSettings GetSettings(byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!int.TryParse(Encoding.Unicode.GetString(key), out int rightShift))
            {
                throw new ArgumentException("Value must be a number.", nameof(key));
            }

            if (rightShift is < 0 or > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(key), "Value must be between 0 and 25.");
            }

            return new CaesarSettings(rightShift);
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