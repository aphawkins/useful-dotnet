// Copyright (c) Andrew Hawkins. All rights reserved.

using System.Security.Cryptography;

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The ROT13 cipher.
    /// </summary>
    public sealed class Rot13Symmetric : SymmetricAlgorithm
    {
        private readonly Rot13 _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rot13Symmetric"/> class.
        /// </summary>
        public Rot13Symmetric()
        {
            Reset();
            _algorithm = new Rot13();
        }

        /// <inheritdoc />
        public override byte[] IV { get => Array.Empty<byte>(); set => _ = value; }

        /// <inheritdoc />
        public override byte[] Key { get => Array.Empty<byte>(); set => _ = value; }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV) => CreateEncryptor(rgbKey, rgbIV);

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            Key = rgbKey;
            IV = rgbIV ?? [];
            return new ClassicalSymmetricTransform(_algorithm, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override void GenerateIV()
        {
            IVValue = [];
            IV = IVValue;
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = [];
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

            KeyValue = [];
            IVValue = [];
        }
    }
}