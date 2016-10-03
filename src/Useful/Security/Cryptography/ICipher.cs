namespace Useful.Security.Cryptography
{
    public interface ICipher
    {
        string Encrypt(string s);

        string CipherName { get; }
    }
}
