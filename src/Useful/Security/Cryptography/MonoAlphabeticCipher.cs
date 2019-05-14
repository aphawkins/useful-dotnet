// <copyright file="MonoAlphabeticCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// The MonoAlphabetic cipher.
    /// </summary>
    public class MonoAlphabeticCipher : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticCipher"/> class.
        /// </summary>
        public MonoAlphabeticCipher()
            : this(new MonoAlphabeticSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticCipher"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public MonoAlphabeticCipher(MonoAlphabeticSettings settings)
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
            ICipher cipher = new MonoAlphabeticCipher(new MonoAlphabeticSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new MonoAlphabeticCipher(new MonoAlphabeticSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override string Decrypt(string ciphertext)
        {
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