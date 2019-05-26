// <copyright file="EnigmaRotorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// Enigma rotor settings.
    /// </summary>
    public class EnigmaRotorSettings : INotifyPropertyChanged
    {
        private readonly IDictionary<EnigmaRotorPosition, EnigmaRotor> _list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();
        private IList<EnigmaRotorNumber> _availableRotors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotorSettings"/> class.
        /// </summary>
        public EnigmaRotorSettings()
        {
            _list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();
            int i = 1;
            foreach (EnigmaRotorPosition position in RotorPositions)
            {
                _list[position] = new EnigmaRotor((EnigmaRotorNumber)i);
                i++;
            }

            PopulateAvailableRotors();
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the rotor positions.
        /// </summary>
        /// <returns>The rotor positions.</returns>
        public static IEnumerable<EnigmaRotorPosition> RotorPositions
        {
            get
            {
                return new List<EnigmaRotorPosition>()
                {
                    EnigmaRotorPosition.Fastest,
                    EnigmaRotorPosition.Second,
                    EnigmaRotorPosition.Third,
                };
            }
        }

        /// <summary>
        /// Gets all the rotors.
        /// </summary>
        /// <returns>All the rotors.</returns>
        public static IEnumerable<EnigmaRotorNumber> RotorSet
        {
            get
            {
                return new List<EnigmaRotorNumber>()
                {
                    EnigmaRotorNumber.I,
                    EnigmaRotorNumber.II,
                    EnigmaRotorNumber.III,
                    EnigmaRotorNumber.IV,
                    EnigmaRotorNumber.V,
                };
            }
        }

        /// <summary>
        /// Gets the available rotors.
        /// </summary>
        public IEnumerable<EnigmaRotorNumber> AvailableRotors
        {
            get
            {
                return _availableRotors;
            }

            private set
            {
                _availableRotors = value.ToList();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Sets the rotor settings.
        /// </summary>
        /// <param name="position">The rotor position to set.</param>
        /// <returns>The rotor to set in this position.</returns>
        public EnigmaRotor this[EnigmaRotorPosition position]
        {
            get
            {
                return _list[position];
            }

            set
            {
                if (!AvailableRotors.ToList().Contains(value.RotorNumber))
                {
                    throw new ArgumentException("This rotor is already in use.", nameof(value));
                }

                _list[position] = value;

                NotifyPropertyChanged();

                PopulateAvailableRotors();
            }
        }

        /// <summary>
        /// Gets the ring key.
        /// </summary>
        /// <returns>The ring key.</returns>
        public string RingKey()
        {
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _list.Reverse().ToArray())
            {
                key.Append(position.Value.RingPosition);
                key.Append(EnigmaSettings.KeyDelimiter);
            }

            if (_list.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        /// <summary>
        /// Gets the rotor order.
        /// </summary>
        /// <returns>The rotor order.</returns>
        public string RotorOrderKey()
        {
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _list.Reverse().ToArray())
            {
                key.Append(position.Value.RotorNumber);
                key.Append(EnigmaSettings.KeyDelimiter);
            }

            if (_list.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        /// <summary>
        /// Gets the settings key.
        /// </summary>
        /// <returns>The settings key.</returns>
        public string SettingKey()
        {
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _list.Reverse().ToArray())
            {
                key.Append(position.Value.CurrentSetting);
                key.Append(EnigmaSettings.KeyDelimiter);
            }

            if (_list.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
            {
                return;
            }

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PopulateAvailableRotors()
        {
            IList<EnigmaRotorNumber> availableRotors = RotorSet.ToList();

            foreach (EnigmaRotorPosition position in RotorPositions)
            {
                if (availableRotors.Contains(_list[position].RotorNumber))
                {
                    availableRotors.Remove(_list[position].RotorNumber);
                }
            }

            AvailableRotors = availableRotors;
        }
    }
}