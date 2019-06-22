// <copyright file="MonoAlphabetic.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA2000 // Dispose objects before losing scope

namespace Useful.Security.Cryptography
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// The MonoAlphabetic cipher.
    /// </summary>
    public class MonoAlphabetic : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabetic"/> class.
        /// </summary>
        public MonoAlphabetic()
            : this(new MonoAlphabeticSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabetic"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public MonoAlphabetic(MonoAlphabeticSettings settings)
            : base("MonoAlphabetic", settings)
        {
            KeyGenerator = new MonoAlphabeticKeyGenerator();
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get => Settings.IV.ToArray();
            set => _ = value;
        }

        /// <inheritdoc />
        public override byte[] Key
        {
            get => Settings.Key.ToArray();

            set
            {
                Settings = new MonoAlphabeticSettings(value);
                base.Key = value;
            }
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new MonoAlphabetic(new MonoAlphabeticSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new MonoAlphabetic(new MonoAlphabeticSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override string Decrypt(string ciphertext)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException(nameof(ciphertext));
            }

            StringBuilder sb = new StringBuilder(ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                sb.Append(((MonoAlphabeticSettings)Settings).Reverse(ciphertext[i]));
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string Encrypt(string plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            StringBuilder sb = new StringBuilder(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                sb.Append(((MonoAlphabeticSettings)Settings)[plaintext[i]]);
            }

            return sb.ToString();
        }

        internal char Decrypt(char ciphertext)
        {
            return ((MonoAlphabeticSettings)Settings).Reverse(ciphertext);
        }

        internal char Encrypt(char plaintext)
        {
            return ((MonoAlphabeticSettings)Settings)[plaintext];
        }
    }
}