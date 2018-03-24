// <copyright file="ROT13Cipher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Text;

    /// <summary>
    /// The ROT13 cipher.
    /// </summary>
    public class ROT13Cipher : ICipher
    {
        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName => "ROT13";

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ICipherSettings Settings { get; set; }

        /// <summary>
        /// Encrypts a plaintext string.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <returns>The encrypted string.</returns>
        public string Encrypt(string plaintext)
        {
            StringBuilder sb = new StringBuilder(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                int c = (int)plaintext[i];

                // Uppercase
                if (c >= (int)'A' && c <= (int)'Z')
                {
                    sb.Append((char)(((c - (int)'A' + 13) % 26) + (int)'A'));
                }

                // Lowercase
                else if (c >= (int)'a' && c <= (int)'z')
                {
                    sb.Append((char)(((c - (int)'a' + 13) % 26) + (int)'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Decrypts a ciphertext string.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string ciphertext)
        {
            return Encrypt(ciphertext);
        }

        /// <summary>
        /// The name of the Cipher.
        /// </summary>
        /// <returns>Name of the Cipher.</returns>
        public override string ToString() => CipherName;
    }
}