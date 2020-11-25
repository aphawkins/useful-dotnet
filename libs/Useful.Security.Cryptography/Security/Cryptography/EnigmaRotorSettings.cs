// <copyright file="EnigmaRotorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enigma rotor settings.
    /// </summary>
    public class EnigmaRotorSettings
    {
        /// <summary>
        /// The seperator between values in a key field.
        /// </summary>
        internal const char KeyDelimiter = ' ';

        private IReadOnlyDictionary<EnigmaRotorPosition, EnigmaRotor> _rotors = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotorSettings"/> class.
        /// </summary>
        public EnigmaRotorSettings() => _rotors = new Dictionary<EnigmaRotorPosition, EnigmaRotor>
            {
                { EnigmaRotorPosition.Fastest, new EnigmaRotor(EnigmaRotorNumber.I, 1, 'A') },
                { EnigmaRotorPosition.Second, new EnigmaRotor(EnigmaRotorNumber.II, 1, 'A') },
                { EnigmaRotorPosition.Third, new EnigmaRotor(EnigmaRotorNumber.III, 1, 'A') },
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotorSettings"/> class.
        /// </summary>
        /// <param name="rotors">The rotors.</param>
        public EnigmaRotorSettings(IReadOnlyDictionary<EnigmaRotorPosition, EnigmaRotor> rotors) => Rotors = rotors;

        /// <summary>
        /// Gets the rotor positions.
        /// </summary>
        /// <returns>The rotor positions.</returns>
        public static IEnumerable<EnigmaRotorPosition> RotorPositions => new List<EnigmaRotorPosition>()
                {
                    EnigmaRotorPosition.Fastest,
                    EnigmaRotorPosition.Second,
                    EnigmaRotorPosition.Third,
                };

        /// <summary>
        /// Gets all the rotors.
        /// </summary>
        /// <returns>All the rotors.</returns>
        public static IEnumerable<EnigmaRotorNumber> RotorSet => new List<EnigmaRotorNumber>()
                {
                    EnigmaRotorNumber.I,
                    EnigmaRotorNumber.II,
                    EnigmaRotorNumber.III,
                    EnigmaRotorNumber.IV,
                    EnigmaRotorNumber.V,
                    EnigmaRotorNumber.VI,
                    EnigmaRotorNumber.VII,
                    EnigmaRotorNumber.VIII,
                };

        /// <summary>
        /// Gets or sets the rotors.
        /// </summary>
        public IReadOnlyDictionary<EnigmaRotorPosition, EnigmaRotor> Rotors
        {
            get => _rotors;
            set
            {
                if (value[EnigmaRotorPosition.Fastest].RotorNumber == value[EnigmaRotorPosition.Second].RotorNumber
                    || value[EnigmaRotorPosition.Fastest].RotorNumber == value[EnigmaRotorPosition.Third].RotorNumber
                    || value[EnigmaRotorPosition.Second].RotorNumber == value[EnigmaRotorPosition.Third].RotorNumber)
                {
                    throw new ArgumentException("This rotor is already in use.", nameof(value));
                }

                _rotors = value;
            }
        }

        /// <summary>
        /// Sets the rotor settings.
        /// </summary>
        /// <param name="position">The rotor position to set.</param>
        /// <returns>The rotor to set in this position.</returns>
        public EnigmaRotor this[EnigmaRotorPosition position]
        {
            get => _rotors[position];

            set
            {
                IReadOnlyDictionary<EnigmaRotorPosition, EnigmaRotor> rotors = new Dictionary<EnigmaRotorPosition, EnigmaRotor>(_rotors)
                {
                    [position] = value,
                };
                Rotors = rotors;
            }
        }

        /// <summary>
        /// Advances the rotor one setting.
        /// </summary>
        public void AdvanceRotors()
        {
            _rotors[EnigmaRotorPosition.Fastest].CurrentSetting = (char)(((_rotors[EnigmaRotorPosition.Fastest].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');

            foreach (char notch in _rotors[EnigmaRotorPosition.Fastest].Notches)
            {
                if ((((_rotors[EnigmaRotorPosition.Fastest].CurrentSetting - 1 - 'A' + 26) % 26) + 'A') == notch)
                {
                    _rotors[EnigmaRotorPosition.Second].CurrentSetting = (char)(((_rotors[EnigmaRotorPosition.Second].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');

                    foreach (char notch2 in _rotors[EnigmaRotorPosition.Second].Notches)
                    {
                        if (_rotors[EnigmaRotorPosition.Second].CurrentSetting - 1 == notch2)
                        {
                            _rotors[EnigmaRotorPosition.Third].CurrentSetting = (char)(((_rotors[EnigmaRotorPosition.Third].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');
                            break;
                        }
                    }
                }

                // Doublestep the middle rotor when the right rotor is 2 past a notch and the middle is on a notch
                if ((((_rotors[EnigmaRotorPosition.Fastest].CurrentSetting - 2) % 'A') + 'A') == notch)
                {
                    foreach (char notch2 in _rotors[EnigmaRotorPosition.Second].Notches)
                    {
                        if (_rotors[EnigmaRotorPosition.Second].CurrentSetting == notch2)
                        {
                            _rotors[EnigmaRotorPosition.Second].CurrentSetting = (char)(((_rotors[EnigmaRotorPosition.Second].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');
                            _rotors[EnigmaRotorPosition.Third].CurrentSetting = (char)(((_rotors[EnigmaRotorPosition.Third].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');
                            break;
                        }
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Gets the ring key.
        /// </summary>
        /// <returns>The ring key.</returns>
        public string RotorRingKey()
        {
            StringBuilder key = new();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _rotors.Reverse().ToArray())
            {
                key.Append($"{position.Value.RingPosition:00}");
                key.Append(KeyDelimiter);
            }

            if (_rotors.Count > 0)
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
            StringBuilder key = new();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _rotors.Reverse().ToArray())
            {
                key.Append(position.Value.RotorNumber);
                key.Append(KeyDelimiter);
            }

            if (_rotors.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        /// <summary>
        /// Gets the settings key.
        /// </summary>
        /// <returns>The settings key.</returns>
        public string RotorSettingKey()
        {
            StringBuilder key = new();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _rotors.Reverse().ToArray())
            {
                key.Append(position.Value.CurrentSetting);
                key.Append(KeyDelimiter);
            }

            if (_rotors.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        /// <summary>
        /// Gets the rotors not being used.
        /// </summary>
        /// <returns>The rotors not in use.</returns>
        public IList<EnigmaRotorNumber> GetAvailableRotors()
        {
            IList<EnigmaRotorNumber> availableRotors = RotorSet.ToList();

            if (_rotors.Any())
            {
                foreach (EnigmaRotorPosition position in RotorPositions)
                {
                    if (!_rotors.ContainsKey(position))
                    {
                        continue;
                    }

                    if (availableRotors.Contains(_rotors[position].RotorNumber))
                    {
                        availableRotors.Remove(_rotors[position].RotorNumber);
                    }
                }
            }

            return availableRotors;
        }
    }
}