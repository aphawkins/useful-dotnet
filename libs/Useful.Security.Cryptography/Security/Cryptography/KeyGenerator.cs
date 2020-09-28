// <copyright file="KeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;

    /// <summary>
    /// Caesar key generator.
    /// </summary>
    internal class KeyGenerator : IKeyGenerator
    {
        /// <inheritdoc />
        public byte[] RandomIv() => Array.Empty<byte>();

        /// <inheritdoc />
        public byte[] RandomKey() => Array.Empty<byte>();
    }
}