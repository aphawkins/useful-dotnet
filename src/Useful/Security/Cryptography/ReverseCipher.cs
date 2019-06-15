// <copyright file="ReverseCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA2000 // Dispose objects before losing scope

namespace Useful.Security.Cryptography
{
    using System.Linq;
    using System.Security.Cryptography;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// The Reverse cipher.
    /// </summary>
    public class ReverseCipher : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseCipher"/> class.
        /// </summary>
        public ReverseCipher()
            : base("Reverse", new CipherSettings())
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override string Decrypt(string ciphertext)
        {
            return Encrypt(ciphertext);
        }

        /// <inheritdoc />
        public override string Encrypt(string plaintext)
        {
            return new string(plaintext.Reverse().ToArray());
        }
    }
}