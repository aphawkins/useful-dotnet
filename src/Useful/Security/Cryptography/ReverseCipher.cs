// <copyright file="ReverseCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// The Reverse cipher.
    /// </summary>
    public class ReverseCipher : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseCipher"/> class.
        /// </summary>
        public ReverseCipher()
            : this(new CipherSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseCipher"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public ReverseCipher(CipherSettings settings)
            : base("Reverse", settings)
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher(new CipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher(new CipherSettings(rgbKey, rgbIV));
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