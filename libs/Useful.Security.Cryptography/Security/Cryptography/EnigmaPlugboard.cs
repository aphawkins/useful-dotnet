// <copyright file="Reflector.cs" company="APH Software">
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
    /// A reflector MonoAlphabetic cipher. A character encrypts and decrypts back to the same character.
    /// </summary>
    public class Reflector : ClassicalSymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Reflector"/> class.
        /// </summary>
        public Reflector()
            : this(new ReflectorSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reflector"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public Reflector(ReflectorSettings settings)
            : base("Reflector", settings)
        {
            KeyGenerator = new ReflectorKeyGenerator();
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get => Settings.IV.ToArray();
            set => _ = value;
        }

        /// <inheritdoc />
        public override byte[] Key
        {
            get => Settings.Key.ToArray();

            set
            {
                Settings = new ReflectorSettings(value);
                base.Key = value;
            }
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            ICipher cipher = new Reflector(new ReflectorSettings(rgbKey));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            ICipher cipher = new Reflector(new ReflectorSettings(rgbKey));
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
                sb.Append(((ReflectorSettings)Settings).Reflect(ciphertext[i]));
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
                sb.Append(((ReflectorSettings)Settings)[plaintext[i]]);
            }

            return sb.ToString();
        }

        internal char Decrypt(char ciphertext)
        {
            return ((ReflectorSettings)Settings).Reflect(ciphertext);
        }

        internal char Encrypt(char plaintext)
        {
            return ((ReflectorSettings)Settings)[plaintext];
        }
    }
}