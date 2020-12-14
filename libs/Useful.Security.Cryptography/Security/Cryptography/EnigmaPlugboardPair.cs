// <copyright file="EnigmaPlugboardPair.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// An Enigma plugboard pair.
    /// </summary>
    public record EnigmaPlugboardPair
    {
        /// <summary>
        /// Gets or sets the plug origin letter.
        /// </summary>
        public char From { get; set; }

        /// <summary>
        /// Gets or sets the plug destination letter.
        /// </summary>
        public char To { get; set; }
    }
}