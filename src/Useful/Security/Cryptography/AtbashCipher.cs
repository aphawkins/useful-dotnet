// <copyright file="AtbashCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Text;

    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public class AtbashCipher : ICipher
    {
        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName => "Atbash";

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
            // To decipher just need to use the encryption method as the cipher is reversible
            return Encrypt(ciphertext);
        }

        /// <summary>
        /// Encrypts a plaintext string.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <returns>The encrypted string.</returns>
        public string Encrypt(string plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            StringBuilder ciphertext = new StringBuilder();

            for (int i = 0; i < plaintext.Length; i++)
            {
                ciphertext.Append(Encipher(plaintext[i]));
            }

            return ciphertext.ToString();
        }

        /// <summary>
        /// The name of the Cipher.
        /// </summary>
        /// <returns>Name of the Cipher.</returns>
        public override string ToString() => CipherName;

        /// <summary>
        /// Encipher a plaintext letter into an enciphered letter.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The enciphered letter.</returns>
        private static char Encipher(char letter)
        {
            if (!char.IsLetter(letter))
            {
                // Not a letter so do nothing to it
                return letter;
            }

            if (char.IsUpper(letter))
            {
                // A=Z, B=Y, C=X, etc
                return (char)('Z' - (letter % 'A'));
            }

            return (char)('z' - (letter % 'a'));
        }
    }
}