// <copyright file="CaesarSettingsObservable.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public sealed class CaesarSettingsObservable : ICaesarSettings, INotifyPropertyChanged
    {
        private readonly CaesarSettings _settings = new CaesarSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSettingsObservable"/> class.
        /// </summary>
        public CaesarSettingsObservable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSettingsObservable"/> class.
        /// </summary>
        /// <param name="rightShift">The right shift.</param>
        public CaesarSettingsObservable(int rightShift) => _settings = new CaesarSettings(rightShift);

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets or sets the right shift of the cipher.
        /// </summary>
        public int RightShift
        {
            get => _settings.RightShift;

            set
            {
                _settings.RightShift = value;

                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Used to raise the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}