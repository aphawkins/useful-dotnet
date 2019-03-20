//-----------------------------------------------------------------------
// <copyright file="MonoAlphabetic.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>Simulates the monoalphabetic substitution cipher.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// Simulates the monoalphabetic substitution cipher.
    /// </summary>
    [DebuggerDisplay("Key-IV={System.Text.Encoding.Unicode.GetString(this.KeyValue)}-{System.Text.Encoding.Unicode.GetString(this.IVValue)}}")]
    public sealed class MonoAlphabetic : SymmetricAlgorithm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabetic"/> class.
        /// </summary>
        public MonoAlphabetic()
        {
            this.ModeValue = CipherMode.ECB;
            this.PaddingValue = PaddingMode.None;
            this.KeySizeValue = int.MaxValue;
            this.BlockSizeValue = 0;

            // FeedbackSizeValue = 2;
            this.LegalBlockSizesValue = new KeySizes[1];
            this.LegalBlockSizesValue[0] = new KeySizes(0, int.MaxValue, 1);
            this.LegalKeySizesValue = new KeySizes[1];
            this.LegalKeySizesValue[0] = new KeySizes(0, int.MaxValue, 1);

            MonoAlphabeticSettingsObservableCollection defaultSettings = MonoAlphabeticSettingsObservableCollection.GetDefault();

            this.KeyValue = new byte[defaultSettings.Key.Count];
            defaultSettings.Key.CopyTo(this.KeyValue, 0);

            this.IVValue = new byte[defaultSettings.IV.Count];
            defaultSettings.IV.CopyTo(this.IVValue, 0);
        }

        /// <summary>
        /// Creates a symmetric decryptor object.
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>The symmetric decryptor object.</returns>
        /// <exception cref="ArgumentNullException"> Thrown if <paramref name="rgbKey" /> is null.</exception>
        /// <exception cref="ArgumentNullException"> Thrown if <paramref name="rgbIV" /> is null.</exception>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (rgbKey == null)
            {
                throw new ArgumentNullException(nameof(rgbKey));
            }

            if (rgbIV == null)
            {
                throw new ArgumentNullException(nameof(rgbIV));
            }

            return new MonoAlphabeticTransform(rgbKey, rgbIV, CipherTransformMode.Decrypt);
        }

        /// <summary>
        /// Creates a symmetric encryptor object.
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>The symmetric encryptor object.</returns>
        /// <exception cref="ArgumentNullException"> Thrown if <paramref name="rgbKey" /> is null.</exception>
        /// <exception cref="ArgumentNullException"> Thrown if <paramref name="rgbIV" /> is null.</exception>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (rgbKey == null)
            {
                throw new ArgumentNullException(nameof(rgbKey));
            }

            if (rgbIV == null)
            {
                throw new ArgumentNullException(nameof(rgbIV));
            }

            return new MonoAlphabeticTransform(rgbKey, rgbIV, CipherTransformMode.Encrypt);
        }

        /// <summary>
        /// Generates a random initialization vector (IV) to use for the algorithm.
        /// </summary>
        public override void GenerateIV()
        {
            byte[] iv = MonoAlphabeticSettingsObservableCollection.GetRandom().IV.ToArray();
            this.IVValue = new byte[iv.Length];
            iv.CopyTo(this.IVValue, 0);
        }

        /// <summary>
        /// Generates a random key to be used for the algorithm.
        /// </summary>
        public override void GenerateKey()
        {
            byte[] key = MonoAlphabeticSettingsObservableCollection.GetRandom().Key.ToArray();
            this.KeyValue = new byte[key.Length];
            key.CopyTo(this.KeyValue, 0);
        }
    }
}