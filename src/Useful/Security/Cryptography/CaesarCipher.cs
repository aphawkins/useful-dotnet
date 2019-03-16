// <copyright file="CaesarCipher.cs" company="APH Software">
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
    public class CaesarCipher : ClassicalSymmetricAlgorithm
    {
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
            : base("Caesar", settings, new CaesarKeyGenerator())
        {
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new CaesarCipher(new CaesarCipherSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new CaesarCipher(new CaesarCipherSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override string Decrypt(string ciphertext)
        {
            StringBuilder sb = new StringBuilder(ciphertext.Length);

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int c = ciphertext[i];

                // Uppercase
                if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(((c - 'A' + 26 - ((CaesarCipherSettings)Settings).RightShift) % 26) + 'A'));
                }

                // Lowercase
                else if (c >= 'a' && c <= 'z')
                {
                    sb.Append((char)(((c - 'a' + 26 - ((CaesarCipherSettings)Settings).RightShift) % 26) + 'a'));
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
            StringBuilder sb = new StringBuilder(plaintext.Length);

            for (int i = 0; i < plaintext.Length; i++)
            {
                int c = plaintext[i];

                // Uppercase
                if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(((c - 'A' + ((CaesarCipherSettings)Settings).RightShift) % 26) + 'A'));
                }

                // Lowercase
                else if (c >= 'a' && c <= 'z')
                {
                    sb.Append((char)(((c - 'a' + ((CaesarCipherSettings)Settings).RightShift) % 26) + 'a'));
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