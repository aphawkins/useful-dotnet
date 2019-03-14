// <copyright file="KeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// Empty key generator.
    /// </summary>
    public class KeyGenerator : IKeyGenerator
    {
        /// <inheritdoc />
        public IEnumerable<byte> DefaultIv => Array.Empty<byte>();

        /// <inheritdoc />
        public IEnumerable<byte> DefaultKey => Array.Empty<byte>();

        /// <inheritdoc />
        public byte[] RandomIv() => Array.Empty<byte>();

        /// <inheritdoc />
        public byte[] RandomKey() => Array.Empty<byte>();
    }
}