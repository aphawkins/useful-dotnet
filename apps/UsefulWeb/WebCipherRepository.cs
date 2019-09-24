// <copyright file="WebCipherRepository.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWeb
{
    using Useful.Security.Cryptography;

    public class WebCipherRepository : CipherRepository
    {
        public WebCipherRepository()
        {
#pragma warning disable IDISP004 // Don't ignore return value of type IDisposable.
#pragma warning disable CA2000 // Dispose objects before losing scope
            Atbash cipher = new Atbash();
            Create(cipher);
            Create(new ROT13());
#pragma warning restore CA2000
#pragma warning restore IDISP004

            CurrentItem = cipher;
        }
    }
}