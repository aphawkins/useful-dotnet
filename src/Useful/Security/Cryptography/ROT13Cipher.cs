// <copyright file="ROT13Cipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// The ROT13 cipher.
    /// </summary>
    public class ROT13Cipher : ClassicalSymmetricAlgorithm, ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ROT13Cipher"/> class.
        /// </summary>
        public ROT13Cipher()
            : this(new CipherSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ROT13Cipher"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public ROT13Cipher(CipherSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName => "ROT13";

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ICipherSettings Settings { get; set; }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ROT13Cipher(new CipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ROT13Cipher(new CipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
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
        /// Encrypts a plaintext string.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <returns>The encrypted string.</returns>
        public string Encrypt(string plaintext)
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
    }
}