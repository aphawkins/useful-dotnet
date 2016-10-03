namespace Useful.Security.Cryptography
{
    using System.Text;

    public class ROT13Cipher : ICipher
    {
        public string Encrypt(string s)
        {
            StringBuilder sb = new StringBuilder(s.Length);

            for (int i = 0; i < s.Length; i++)
            {
                int c = (int)s[i];
                // Uppercase
                if (c >= (int)'A' && c <= (int)'Z')
                {
                    sb.Append((char)(((c - (int)'A' + 13) % 26) + (int)'A'));
                }
                // Lowercase
                else if (c >= (int)'a' && c <= (int)'z')
                {
                    sb.Append((char)(((c - (int)'a' + 13) % 26) + (int)'a'));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public string CipherName { get { return "ROT13"; } }
    }
}
