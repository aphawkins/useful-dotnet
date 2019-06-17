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

        internal EnigmaRotorSettings(string rotorOrder, string ringSettings, string rotorSettings)
        {
            _list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();
            PopulateAvailableRotors();

            // Rotor Order
            string[] rotors = rotorOrder.Split(new char[] { ' ' });
            if (rotors.Length <= 0)
            {
                throw new ArgumentException("No rotors specified.");
            }

            int rotorPositionsCount = RotorPositions.Count();

            if (rotors.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rotors specified.");
            }

            if (rotors.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rotors specified.");
            }

            if (rotors[0].Length == 0)
            {
                throw new ArgumentException("No rotors specified.");
            }

            rotors = rotors.Reverse().ToArray();

            // Ring
            string[] rings = ringSettings.Split(new char[] { ' ' });

            if (rings.Length <= 0)
            {
                throw new ArgumentException("No rings specified.");
            }

            if (rings.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rings specified.");
            }

            if (rings.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rings specified.");
            }

            if (rings[0].Length == 0)
            {
                throw new ArgumentException("No rings specified.");
            }

            rings = rings.Reverse().ToArray();

            // Rotor Settings
            string[] rotorSetting = rotorSettings.Split(new char[] { ' ' });

            if (rotorSetting.Length <= 0)
            {
                throw new ArgumentException("No rotor settings specified.");
            }

            if (rotorSetting.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rotor settings specified.");
            }

            if (rotorSetting.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rotor settings specified.");
            }

            if (rotorSetting[0].Length == 0)
            {
                throw new ArgumentException("No rotor settings specified.");
            }

            rotorSetting = rotorSetting.Reverse().ToArray();

            for (int i = 0; i < rotors.Length; i++)
            {
                if (string.IsNullOrEmpty(rotors[i]) || rotors[i].Contains("\0"))
                {
                    throw new ArgumentException("Null or empty rotor specified.");
                }

                if (!Enum.TryParse<EnigmaRotorPosition>(i.ToString(), out EnigmaRotorPosition rotorPosition))
                {
                    throw new ArgumentException($"Invalid rotor position {i}.");
                }

                if (!Enum.TryParse<EnigmaRotorNumber>(rotors[i], out EnigmaRotorNumber rotorNumber))
                {
                    throw new ArgumentException($"Invalid rotor number {rotors[i]}.");
                }

                if (!AvailableRotors.Contains(rotorNumber))
                {
                    throw new ArgumentException("This rotor is already in use.");
                }

                EnigmaRotor enigmaRotor = new EnigmaRotor(rotorNumber)
                {
                    RingPosition = int.Parse(rings[i]),
                };

                if (rotorSetting[i].Length > 1)
                {
                    throw new ArgumentException("Invalid rotor number setting.");
                }

                enigmaRotor.CurrentSetting = rotorSetting[i][0];
                enigmaRotor.RotorAdvanced += EnigmaRotorSettings_RotorAdvanced;

                if (_list.ContainsKey(rotorPosition))
                {
                    _list[rotorPosition] = enigmaRotor;
                }
                else
                {
                    _list.Add(new KeyValuePair<EnigmaRotorPosition, EnigmaRotor>(rotorPosition, enigmaRotor));
                }

                PopulateAvailableRotors();
            }
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
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (!AvailableRotors.ToList().Contains(value.RotorNumber))
                {
                    throw new ArgumentException("This rotor is already in use.", nameof(value));
                }

                _list[position] = value;
                _list[position].RotorAdvanced += EnigmaRotorSettings_RotorAdvanced;

                PopulateAvailableRotors();
                NotifyPropertyChanged();
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
                key.Append($"{position.Value.RingPosition:00}");
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

        private void EnigmaRotorSettings_RotorAdvanced(object sender, EnigmaRotorAdvanceEventArgs e)
        {
            if (e.IsNotchHit)
            {
                if (_list[EnigmaRotorPosition.Fastest].RotorNumber == e.RotorNumber)
                {
                    _list[EnigmaRotorPosition.Second].AdvanceRotor();
                }
                else if (_list[EnigmaRotorPosition.Second].RotorNumber == e.RotorNumber)
                {
                    _list[EnigmaRotorPosition.Third].AdvanceRotor();
                }
            }

            if (e.IsDoubleStep)
            {
                if (_list[EnigmaRotorPosition.Fastest].RotorNumber == e.RotorNumber)
                {
                    _list[EnigmaRotorPosition.Second].AdvanceRotor();
                }
            }
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

            if (_list.Count() > 0)
            {
                foreach (EnigmaRotorPosition position in RotorPositions)
                {
                    if (!_list.ContainsKey(position))
                    {
                        continue;
                    }

                    if (availableRotors.Contains(_list[position].RotorNumber))
                    {
                        availableRotors.Remove(_list[position].RotorNumber);
                    }
                }
            }

            AvailableRotors = availableRotors;
        }
    }
}