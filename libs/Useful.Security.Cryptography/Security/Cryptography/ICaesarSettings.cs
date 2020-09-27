// <copyright file="ICaesarSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Caesar cipher settings.
    /// </summary>
    public interface ICaesarSettings
    {
        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public int RightShift
        {
            get;
            set;
        }
    }
}