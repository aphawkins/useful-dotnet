// <copyright file="AtbashCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// The Atbash cipher.
    /// </summary>
    public class AtbashCipher : ClassicalSymmetricAlgorithm, ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AtbashCipher"/> class.
        /// </summary>
        public AtbashCipher()
            : this(new CipherSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AtbashCipher"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public AtbashCipher(CipherSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName => "Atbash";

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ICipherSettings Settings { get; set; }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new AtbashCipher(new CipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new AtbashCipher(new CipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

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

        /// <inheritdoc />
        public override void GenerateIV()
        {
            // IV is always empty.
            IVValue = Settings.KeyGenerator.RandomIv();
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = Settings.KeyGenerator.RandomKey();
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