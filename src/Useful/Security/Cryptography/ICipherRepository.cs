namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// Retrieves all the ciphers.
    /// </summary>
    /// <returns>All the ciphers.</returns>
    public interface ICipherRepository
    {
        /// <summary>
        /// Retrieves all the ciphers.
        /// </summary>
        /// <returns>All the ciphers.</returns>
        List<ICipher> GetCiphers();
    }
}