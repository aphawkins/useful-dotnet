// <copyright file="EnigmaRotorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// Enigma rotor settings.
    /// </summary>
    public class EnigmaRotorSettings : INotifyPropertyChanged
    {
        private readonly IList<EnigmaRotorNumber> _availableRotors;
        private readonly IDictionary<EnigmaRotorPosition, EnigmaRotor> _list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotorSettings"/> class.
        /// </summary>
        public EnigmaRotorSettings()
        {
            _availableRotors = GetAllowedRotors();

            _list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();
            int i = 1;
            foreach (EnigmaRotorPosition position in GetAllowedRotorPositions())
            {
                _list[position] = new EnigmaRotor((EnigmaRotorNumber)i);
                i++;
            }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the available rotors.
        /// </summary>
        public IEnumerable<EnigmaRotorNumber> AvailableRotors123
        {
            get
            {
                return _availableRotors;
            }
        }

        /// <summary>
        /// Gets the number of rotors.
        /// </summary>
        public int Count => _list.Count;

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
                _list[position] = value;

                NotifyPropertyChanged();

                // Correct the available rotors list
                if (_availableRotors.Contains(value.RotorNumber))
                {
                    _availableRotors.Remove(value.RotorNumber);
                    NotifyPropertyChanged(nameof(AvailableRotors123));
                }
            }
        }

        /// <summary>
        /// Get the allowed rotor positions.
        /// </summary>
        /// <returns>The allowed rotor positions.</returns>
        public static IList<EnigmaRotorPosition> GetAllowedRotorPositions()
        {
            IList<EnigmaRotorPosition> allowedRotorPositions = new List<EnigmaRotorPosition>()
            {
                EnigmaRotorPosition.Fastest,
                EnigmaRotorPosition.Second,
                EnigmaRotorPosition.Third,
            };

            return allowedRotorPositions;
        }

        /// <summary>
        /// Gets all the allowed rotors for a given Enigma machine type.
        /// </summary>
        /// <returns>All the allowed rotors for the machine type and position.</returns>
        public static IList<EnigmaRotorNumber> GetAllowedRotors()
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

        /// <summary>
        /// Returns the available rotors for a given Enigma model and given position.
        /// </summary>
        /// <returns>The available rotors for a given Enigma model and given position.</returns>
        public IEnumerable<EnigmaRotorNumber> GetAvailableRotors()
        {
            // Contract.Assert(AllowedRotorPositions.Contains(rotorPosition));
            IList<EnigmaRotorNumber> availableRotors = GetAllowedRotors();

            foreach (EnigmaRotorPosition position in GetAllowedRotorPositions())
            {
                if (availableRotors.Contains(_list[position].RotorNumber))
                {
                    availableRotors.Remove(_list[position].RotorNumber);
                }
            }

            return availableRotors;
        }

        /// <summary>
        /// Gets the key order.
        /// </summary>
        /// <returns>The key order.</returns>
        public string GetOrderKey()
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
        /// Gets the ring key.
        /// </summary>
        /// <returns>The ring key.</returns>
        public string GetRingKey()
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
        /// Gets the settings key.
        /// </summary>
        /// <returns>The settings key.</returns>
        public string GetSettingKey()
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

        /////// <summary>
        ///////
        /////// </summary>
        /////// <param name="rotorOrder"></param>
        /////// <returns></returns>
        //// private static EnigmaRotorSettings GetDefaultRotorSettings(EnigmaRotorSettings rotorOrder)
        //// {
        ////    Contract.Requires(rotorOrder != null);

        //// SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSetting = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings>();

        //// foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorSettings> rotor in rotorOrder)
        ////    {
        ////        Collection<char> letters = EnigmaRotor.GetAllowedLetters(rotor.Value.RotorNumber);
        ////        EnigmaRotorSettings setting = new EnigmaRotorSettings(rotor.Value.RotorNumber, letters[0], letters[0]);
        ////        rotorSetting.Add(rotor.Key, setting);
        ////    }

        //// return rotorSetting;
        //// }

        /////// <summary>
        ///////
        /////// </summary>
        /////// <param name="model"></param>
        /////// <returns></returns>
        ////public static EnigmaRotorSettings GetDefault(EnigmaModel model)
        ////{
        ////    EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.Create(model);
        ////    rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.Three);
        ////    rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.Two);
        ////    rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.One);
        ////    switch (model)
        ////    {
        ////        case EnigmaModel.Military:
        ////            break;
        ////        case EnigmaModel.M3:
        ////        case EnigmaModel.M4:
        ////            rotorSettings[EnigmaRotorPosition.Forth] = EnigmaRotor.Create(EnigmaRotorNumber.Beta);
        ////            break;
        ////        default:
        ////            throw new ArgumentException("Unknown Enigma model.");
        ////    }

        ////    return rotorSettings;
        ////}
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
    }
}