// <copyright file="ReverseCipher.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Linq;
    using System.Security.Cryptography;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// The Reverse cipher.
    /// </summary>
    public class ReverseCipher : ClassicalSymmetricAlgorithm, ICipher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseCipher"/> class.
        /// </summary>
        public ReverseCipher()
            : this(new CipherSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReverseCipher"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public ReverseCipher(CipherSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName => "Reverse";

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public ICipherSettings Settings { get; set; }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher(new CipherSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new ReverseCipher(new CipherSettings(rgbKey, rgbIV));
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
        /// <returns>The encrypted text.</returns>
        public string Encrypt(string plaintext)
        {
            return new string(plaintext.Reverse().ToArray());
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