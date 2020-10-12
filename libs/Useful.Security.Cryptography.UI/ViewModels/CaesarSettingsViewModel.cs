// <copyright file="CaesarSettingsViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Settings for the Caesar cipher.
    /// </summary>
    public sealed class CaesarSettingsViewModel : ICipherSettingsViewModel, INotifyPropertyChanged
    {
        private readonly ICaesarSettings _settings = new CaesarSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="CaesarSettingsViewModel"/> class.
        /// </summary>
        /// <param name="settings">The cipher settings.</param>
        public CaesarSettingsViewModel(ICaesarSettings settings) => _settings = settings;

        /////// <summary>
        /////// Initializes a new instance of the <see cref="CaesarSettingsObservable"/> class.
        /////// </summary>
        /////// <param name="rightShift">The right shift.</param>
        ////public CaesarSettingsObservable(int rightShift) => _settings = new CaesarSettings(rightShift);

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