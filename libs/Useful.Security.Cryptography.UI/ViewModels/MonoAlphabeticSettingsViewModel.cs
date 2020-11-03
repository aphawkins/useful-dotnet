// <copyright file="MonoAlphabeticSettingsViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Useful.Security.Cryptography.UI.ViewModels;

    /// <summary>
    /// The monoalphabetic algorithm settings.
    /// </summary>
    public sealed class MonoAlphabeticSettingsViewModel : ICipherSettingsViewModel, INotifyPropertyChanged
    {
        private readonly IMonoAlphabeticSettings _settings = new MonoAlphabeticSettings();

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticSettingsViewModel"/> class.
        /// </summary>
        /// <param name="settings">The cipher settings.</param>
        public MonoAlphabeticSettingsViewModel(IMonoAlphabeticSettings settings) => _settings = settings;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets the character set.
        /// </summary>
        /// <value>The character set.</value>
        public string CharacterSet => _settings.CharacterSet;

        /// <summary>
        /// Gets substitutions.
        /// </summary>
        public string Substitutions => _settings.Substitutions;

        /// <summary>
        /// Gets the number of substitutions made. One distinct pair swapped equals one substitution.
        /// </summary>
        /// <value>The number of distinct substitutions.</value>
        /// <returns>The number of distinct substitutions made.</returns>
        public int SubstitutionCount => _settings.SubstitutionCount;

        /// <summary>
        /// Gets or sets the current substitutions.
        /// </summary>
        /// <param name="substitution">The position to set.</param>
        public char this[char substitution]
        {
            get => _settings[substitution];

            set
            {
                if (_settings[substitution] == value)
                {
                    return;
                }

                _settings[substitution] = value;

                NotifyPropertyChanged("Item");
            }
        }

        /// <summary>
        /// Used to raise the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}