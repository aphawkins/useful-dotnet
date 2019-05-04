// <copyright file="MonoAlphabeticCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;

    /// <summary>
    /// The MonoAlphabetic cipher.
    /// </summary>
    public class MonoAlphabeticCipher : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticCipher"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public MonoAlphabeticCipher(MonoAlphabeticSettings settings)
            : base("Caesar", settings)
        {
            KeyGenerator = new MonoAlphabeticKeyGenerator();
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override string Decrypt(string ciphertext)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public override string Encrypt(string plaintext)
        {
            throw new System.NotImplementedException();
        }
    }
}