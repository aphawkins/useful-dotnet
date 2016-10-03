namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    public class CipherRepository
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

        public string Encrypt(ICipher cipher, string plaintext)
        {
            return cipher.Encrypt(plaintext);
        }
    }
}