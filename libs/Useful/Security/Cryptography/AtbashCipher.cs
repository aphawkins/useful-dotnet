// <copyright file="AtbashCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA2000 // Dispose objects before losing scope

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public class AtbashCipher : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtbashCipher"/> class.
        /// </summary>
        public AtbashCipher()
            : base("Atbash", new CipherSettings())
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new AtbashCipher();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new AtbashCipher();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override string Decrypt(string ciphertext)
        {
            // To decipher just need to use the encryption method as the cipher is reversible
            return Encrypt(ciphertext);
        }

        /// <inheritdoc />
        public override string Encrypt(string plaintext)
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
        /// Encipher a plaintext letter into an enciphered letter.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The enciphered letter.</returns>
        private static char Encipher(char letter)
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