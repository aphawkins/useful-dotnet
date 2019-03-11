// <copyright file="CipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Settings for the Reverse cipher.
    /// </summary>
    public abstract class CipherSettings : ICipherSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CipherSettings"/> class.
        /// </summary>
        public CipherSettings()
        {
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Used to raise the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}