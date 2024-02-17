// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Security.Cryptography.UI.Services
{
    /// <summary>
    /// A viewmodel for ciphers.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CipherService"/> class.
    /// </remarks>
    /// <param name="repository">The repository holding the ciphers.</param>
    public class CipherService(IRepository<ICipher> repository)
    {

        /// <summary>
        /// Gets or sets the cipher repository.
        /// </summary>
        public IRepository<ICipher> Repository { get; set; } = repository;

        /// <summary>
        /// Used to encrypt the Plaintext into Ciphertext.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <returns>The encrypted text.</returns>
        public string Encrypt(string plaintext)
            => Repository.CurrentItem == null || string.IsNullOrEmpty(plaintext) ? string.Empty : Repository.CurrentItem.Encrypt(plaintext);

        /// <summary>
        /// Used to decrypt the Ciphertext into Plaintext.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string ciphertext)
            => Repository.CurrentItem == null || string.IsNullOrEmpty(ciphertext)
                ? string.Empty
                : Repository.CurrentItem.Decrypt(ciphertext);
    }
}
