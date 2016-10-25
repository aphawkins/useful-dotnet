namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds all the ciphers.
    /// </summary>
    public class CipherRepository : ICipherRepository
    {
        private List<ICipher> ciphers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherRepository"/> class.
        /// </summary>
        public CipherRepository()
        {
            this.ciphers = new List<ICipher>
            {
                new ReverseCipher(),
                new ROT13Cipher()
            };
        }

        /// <summary>
        /// Retrieves all the ciphers.
        /// </summary>
        /// <returns>All the ciphers.</returns>
        public List<ICipher> GetCiphers()
        {
            return this.ciphers;
        }
    }
}