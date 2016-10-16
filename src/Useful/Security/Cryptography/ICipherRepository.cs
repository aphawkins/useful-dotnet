namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    public interface ICipherRepository
    {
        List<ICipher> GetCiphers();
    }
}