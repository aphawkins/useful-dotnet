// <copyright file="Atbash.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA2000 // Dispose objects before losing scope

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public class Atbash : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Atbash"/> class.
        /// </summary>
        public Atbash()
            : base("Atbash", new CipherSettings())
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new Atbash();
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new Atbash();
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
                ciphertext.Append(Encrypt(plaintext[i]));
            }

            return ciphertext.ToString();
        }

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