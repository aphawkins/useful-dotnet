// <copyright file="MonoAlphabeticSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The MonoAlphabetic cipher.
    /// </summary>
    public class MonoAlphabeticSymmetric : SymmetricAlgorithm
    {
        /// <summary>
        /// States how many parts there are in the key.
        /// </summary>
        private const int KeyParts = 2;

        /// <summary>
        /// The char that separates part of the key.
        /// </summary>
        private const char KeySeperator = '|';

        /// <summary>
        /// The encoding used by this cipher.
        /// </summary>
        private static readonly Encoding Encoding = new UnicodeEncoding(false, false);

        private readonly MonoAlphabetic _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSymmetric"/> class.
        /// </summary>
        public MonoAlphabeticSymmetric()
        {
            Reset();
            _algorithm = new MonoAlphabetic(new MonoAlphabeticSettings());
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get => Array.Empty<byte>();
            set => _ = value;
        }

        /// <inheritdoc />
        public override byte[] Key
        {
            get
            {
                // CharacterSet|Substitutions
                StringBuilder key = new StringBuilder(_algorithm.Settings.CharacterSet);
                key.Append(KeySeperator);
                key.Append(_algorithm.Settings.Substitutions);

                return Encoding.GetBytes(key.ToString());
            }

            set
            {
                (string CharacterSet, string Substitutions) key;
                try
                {
                    key = ParseKey(value);

                    ParseCharacterSet(key.CharacterSet);
                    ParseSubstitutions(key.CharacterSet, key.Substitutions);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Argument exception.", nameof(value), ex);
                }

                _algorithm.Settings = new MonoAlphabeticSettings(key.CharacterSet, key.Substitutions);
                base.Key = value;
            }
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            (string characterSet, string substitutions) = ParseKey(rgbKey);
            ParseCharacterSet(characterSet);
            ParseSubstitutions(characterSet, substitutions);
            ICipher cipher = new MonoAlphabetic(new MonoAlphabeticSettings(characterSet, substitutions));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            (string characterSet, string substitutions) = ParseKey(rgbKey);
            ParseCharacterSet(characterSet);
            ParseSubstitutions(characterSet, substitutions);
            ICipher cipher = new MonoAlphabetic(new MonoAlphabeticSettings(characterSet, substitutions));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
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
            IMonoAlphabeticSettings settings = MonoAlphabeticSettingsGenerator.Generate();
            string key = settings.CharacterSet + KeySeperator + settings.Substitutions;
            KeyValue = Encoding.GetBytes(key);
            Key = KeyValue;
        }

        /// <inheritdoc/>
        public override string ToString() => _algorithm.CipherName;

        private static (string CharacterSet, string Substitutions) ParseKey(byte[] key)
        {
            // Example:
            // characterSet|substitutions
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.SequenceEqual(Array.Empty<byte>()))
            {
                throw new ArgumentException("Invalid format.", nameof(key));
            }

            string keyString = Encoding.GetString(key);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.", nameof(key));
            }

            return (parts[0], parts[1]);
        }

        private static string ParseCharacterSet(string characterSet)
        {
            if (string.IsNullOrWhiteSpace(characterSet))
            {
                throw new ArgumentException("Invalid number of characters.", nameof(characterSet));
            }

            foreach (char character in characterSet)
            {
                if (!char.IsLetter(character))
                {
                    throw new ArgumentException("All characters must be letters.", nameof(characterSet));
                }
            }

            if (characterSet.Length != characterSet.Distinct().Count())
            {
                throw new ArgumentException("Characters must not be duplicated.", nameof(characterSet));
            }

            return characterSet;
        }

        private static string ParseSubstitutions(string characterSet, string substitutions)
        {
            if (string.IsNullOrWhiteSpace(substitutions)
                || substitutions.Length != characterSet.Length)
            {
                throw new ArgumentException("Incorrect number of substitutions.", nameof(substitutions));
            }

            foreach (char character in substitutions)
            {
                if (!char.IsLetter(character))
                {
                    throw new ArgumentException("All substitutions must be letters.", nameof(substitutions));
                }
            }

            if (substitutions.Length != substitutions.Distinct().Count())
            {
                throw new ArgumentException("Substitutions must not be duplicated.", nameof(substitutions));
            }

            if (!substitutions.All(x => characterSet.Contains(x)))
            {
                throw new ArgumentException("Substitutions must be in the character set.", nameof(substitutions));
            }

            return substitutions;
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