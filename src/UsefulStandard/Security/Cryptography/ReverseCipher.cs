namespace Useful.Security.Cryptography
{
    using System.ComponentModel;
    using System.Linq;

    public class ReverseCipher : ICipher
    {
        public string Encrypt(string s)
        {
            return new string(s.Reverse().ToArray());
        }

        public string CipherName { get { return "Reverse"; } }
    }
}