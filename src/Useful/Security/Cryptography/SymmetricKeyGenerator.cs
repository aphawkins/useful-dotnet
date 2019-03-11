// <copyright file="SymmetricKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using Useful.Interfaces.Security.Cryptography;

    internal class SymmetricKeyGenerator : ISymmetricKeyGenerator
    {
        public IEnumerable<byte> DefaultIv => Array.Empty<byte>();

        public IEnumerable<byte> DefaultKey => Array.Empty<byte>();

        public byte[] RandomIv() => Array.Empty<byte>();

        public byte[] RandomKey() => Array.Empty<byte>();
    }
}