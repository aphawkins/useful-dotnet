// <copyright file="ROT13Cipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// The ROT13 cipher.
    /// </summary>
    public class ROT13Cipher : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ROT13Cipher"/> class.
        /// </summary>
        public ROT13Cipher()
            : base("ROT13", new CipherSettings())
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ROT13Cipher();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ROT13Cipher();
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
            StringBuilder sb = new StringBuilder(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                int c = plaintext[i];

                // Uppercase
                if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(((c - 'A' + 13) % 26) + 'A'));
                }

                // Lowercase
                else if (c >= 'a' && c <= 'z')
                {
                    sb.Append((char)(((c - 'a' + 13) % 26) + 'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }
    }
}