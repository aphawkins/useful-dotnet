﻿// <copyright file="CaesarCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// The Caesar cipher.
    /// </summary>
    public class CaesarCipher : ClassicalSymmetricAlgorithm, ICipher
    {
        private readonly IKeyGenerator _keyGen = new CaesarKeyGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipher"/> class.
        /// </summary>
        public CaesarCipher()
            : this(new CaesarCipherSettings(0))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipher"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public CaesarCipher(CaesarCipherSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName => "Caesar";

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ICipherSettings Settings { get; set; }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new CaesarCipher(new CaesarCipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new CaesarCipher(new CaesarCipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

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

        /// <inheritdoc />
        public override void GenerateIV()
        {
            // IV is always empty.
            IVValue = _keyGen.RandomIv();
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            KeyValue = _keyGen.RandomKey();
        }

        /// <summary>
        /// The name of the Cipher.
        /// </summary>
        /// <returns>Name of the Cipher.</returns>
        public override string ToString() => CipherName;
    }
}