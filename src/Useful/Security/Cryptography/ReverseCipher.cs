namespace Useful.Security.Cryptography
{
    using System.Linq;

    /// <summary>
    /// The Reverse cipher.
    /// </summary>
    public class ReverseCipher : ICipher
    {
        /// <summary>
        /// Gets the name of this cipher.
        /// </summary>
        public string CipherName
        {
            get
            {
                return "Reverse";
            }
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

        /// <summary>
        /// Decrypts a ciphertext string.
        /// </summary>
        /// <param name="ciphertext">The text to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        public string Decrypt(string ciphertext)
        {
            return this.Encrypt(ciphertext);
        }
    }
}