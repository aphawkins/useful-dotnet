// <copyright file="IVigenereSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Caesar cipher settings.
    /// </summary>
    public interface IVigenereSettings
    {
        /// <summary>
        /// Gets or sets the keyword of the cipher.
        /// </summary>
        public string Keyword
        {
            get;
            set;
        }
    }
}