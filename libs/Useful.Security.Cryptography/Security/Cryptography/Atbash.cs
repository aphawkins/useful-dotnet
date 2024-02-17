// <copyright file="Atbash.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using System.Text;

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public sealed class Atbash : ICipher
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

            StringBuilder ciphertext = new();

            for (int i = 0; i < plaintext.Length; i++)
            {
                ciphertext.Append(Encrypt(plaintext[i]));
            }

            return ciphertext.ToString();
        }

        /// <inheritdoc />
        public override string ToString() => CipherName;

        private static char Encrypt(char letter)
        {
            if (letter is >= 'A' and <= 'Z')
            {
                // A=Z, B=Y, C=X, etc
                return (char)('Z' - (letter % 'A'));
            }
            else if (letter is >= 'a' and <= 'z')
            {
                return (char)('Z' - (letter % 'a'));
            }
            else
            {
                return letter;
            }
        }
    }
}
