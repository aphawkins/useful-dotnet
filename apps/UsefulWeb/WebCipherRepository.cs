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
            Atbash cipher = new();
            Create(cipher);
            Create(new ROT13());
            CurrentItem = cipher;
        }
    }
}