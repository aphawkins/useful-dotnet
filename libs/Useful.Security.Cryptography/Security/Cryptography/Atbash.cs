// <copyright file="Atbash.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Text;

    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public class Atbash : ICipher
    {
        /// <inheritdoc />
        public string CipherName => "Atbash";

        /// <inheritdoc />
        public string Decrypt(string ciphertext) =>
            Encrypt(ciphertext); // To decipher just need to use the encryption method as the cipher is reversible

        /// <inheritdoc />
        public string Encrypt(string plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            StringBuilder ciphertext = new StringBuilder();

            for (int i = 0; i < plaintext.Length; i++)
            {
                ciphertext.Append(Encrypt(plaintext[i]));
            }

            return ciphertext.ToString();
        }

        /// <inheritdoc/>
        public override string ToString() => CipherName;

        private static char Encrypt(char letter)
        {
            if (letter >= 'A' && letter <= 'Z')
            {
                // A=Z, B=Y, C=X, etc
                return (char)('Z' - (letter % 'A'));
            }
            else if (letter >= 'a' && letter <= 'z')
            {
                return (char)('z' - (letter % 'a'));
            }
            else
            {
                return letter;
            }
        }
    }
}