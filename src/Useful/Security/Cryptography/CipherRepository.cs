namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    public class CipherRepository : ICipherRepository
    {
        private List<ICipher> ciphers;

        public CipherRepository()
        {
            ciphers = new List<ICipher>
            {
                new ROT13Cipher(),
                new ReverseCipher()
            };
        }

        public List<ICipher> GetCiphers()
        {
            return ciphers;
        }
    }
}