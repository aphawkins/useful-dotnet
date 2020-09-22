// <copyright file="Caesar.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA2000 // Dispose objects before losing scope

namespace Useful.Security.Cryptography
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The Caesar cipher.
    /// </summary>
    public class Caesar : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Caesar"/> class.
        /// </summary>
        public Caesar()
            : this(new CaesarSettings(0))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Caesar"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public Caesar(CaesarSettings settings)
            : base("Caesar", settings)
        {
            KeyGenerator = new CaesarKeyGenerator();
        }

        /// <inheritdoc />
        public override byte[] Key
        {
            get => Settings.Key.ToArray();

            set
            {
                Settings = new CaesarSettings(value);
                base.Key = value;
            }
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get => Settings.IV.ToArray();
            set => _ = value;
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            ICipher cipher = new Caesar(new CaesarSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            ICipher cipher = new Caesar(new CaesarSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override string Decrypt(string ciphertext)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException(nameof(ciphertext));
            }

            StringBuilder sb = new StringBuilder(ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int c = ciphertext[i];

                // Uppercase
                if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(((c - 'A' + 26 - ((CaesarSettings)Settings).RightShift) % 26) + 'A'));
                }

                // Lowercase
                else if (c >= 'a' && c <= 'z')
                {
                    sb.Append((char)(((c - 'a' + 26 - ((CaesarSettings)Settings).RightShift) % 26) + 'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public override string Encrypt(string plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            StringBuilder sb = new StringBuilder(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                int c = plaintext[i];

                // Uppercase
                if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(((c - 'A' + ((CaesarSettings)Settings).RightShift) % 26) + 'A'));
                }

                // Lowercase
                else if (c >= 'a' && c <= 'z')
                {
                    sb.Append((char)(((c - 'a' + ((CaesarSettings)Settings).RightShift) % 26) + 'a'));
                }
                else
                {
                    sb.Append((char)c);
                }
            }

            return sb.ToString();
        }
    }
}