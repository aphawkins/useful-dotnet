// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Settings for the Vigenere cipher.
    /// </summary>
    public sealed record VigenereSettings : IVigenereSettings
    {
        /// <summary>
        /// The keyword.
        /// </summary>
        private string _keyword = string.Empty;

        /// <summary>
        /// Gets or sets the keyword of the cipher.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Length must be between 0 and 26 letters.</exception>
        public string Keyword
        {
            get => _keyword;

            set
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length is < 0 or > 25)
                {
                    throw new ArgumentOutOfRangeException(nameof(Keyword), "Length must be between 0 and 26 letters.");
                }

                _keyword = value.ToUpperInvariant();
            }
        }
    }
}
