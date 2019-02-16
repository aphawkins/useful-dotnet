// <copyright file="CaesarCipherSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public class CaesarCipherSettings : ICipherSettings
    {
        /// <summary>
        /// The right shift.
        /// </summary>
        private int _rightShift;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarCipherSettings"/> class.
        /// </summary>
        /// <param name="rightShift">The right shift.</param>
        public CaesarCipherSettings(int rightShift)
        {
            ValidateRightShift(rightShift);

            _rightShift = rightShift;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public virtual int RightShift
        {
            get
            {
                return _rightShift;
            }

            set
            {
                ValidateRightShift(value);

                _rightShift = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Used to raise the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void ValidateRightShift(int value)
        {
            if (value < 0 || value > 25)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 25.");
            }
        }
    }
}