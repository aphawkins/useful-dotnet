// <copyright file="CaesarCipher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Text;

    /// <summary>
    /// The Caesar cipher.
    /// </summary>
    public class CaesarCipher : ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipher"/> class.
        /// </summary>
        public CaesarCipher()
        {
            Settings = new CaesarCipherSettings();
        }

        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName => "Caesar";

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ICipherSettings Settings { get; set; }

        /// <summary>
        /// Decrypts a ciphertext string.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string ciphertext)
        {
            StringBuilder sb = new StringBuilder(ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int c = (int)ciphertext[i];

                // Uppercase
                if (c >= (int)'A' && c <= (int)'Z')
                {
                    sb.Append((char)(((c - (int)'A' + 26 - ((CaesarCipherSettings)Settings).RightShift) % 26) + (int)'A'));
                }

                // Lowercase
                else if (c >= (int)'a' && c <= (int)'z')
                {
                    sb.Append((char)(((c - (int)'a' + 26 - ((CaesarCipherSettings)Settings).RightShift) % 26) + (int)'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }

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
                    sb.Append((char)(((c - (int)'A' + ((CaesarCipherSettings)Settings).RightShift) % 26) + (int)'A'));
                }

                // Lowercase
                else if (c >= (int)'a' && c <= (int)'z')
                {
                    sb.Append((char)(((c - (int)'a' + ((CaesarCipherSettings)Settings).RightShift) % 26) + (int)'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// The name of the Cipher.
        /// </summary>
        /// <returns>Name of the Cipher.</returns>
        public override string ToString() => CipherName;
    }
}