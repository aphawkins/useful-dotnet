// <copyright file="MonoAlphabeticCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using System.Text;

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
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string Encrypt(string plaintext)
        {
            StringBuilder sb = new StringBuilder(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
            }

            return sb.ToString();
        }
    }
}