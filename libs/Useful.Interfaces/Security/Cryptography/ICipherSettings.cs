// <copyright file="ICipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Interface that all cipher settings should implement.
    /// </summary>
    public interface ICipherSettings : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the encryption Key.
        /// </summary>
        /// <value>The encryption Key.</value>
        /// <returns>Encryption Key.</returns>
        IEnumerable<byte> Key
        {
            get;
        }

        /// <summary>
        /// Gets the Initialization Vector.
        /// </summary>
        /// <value>The Initialization Vector.</value>
        /// <returns>Initialization Vector.</returns>
        IEnumerable<byte> IV
        {
            get;
        }
    }
}