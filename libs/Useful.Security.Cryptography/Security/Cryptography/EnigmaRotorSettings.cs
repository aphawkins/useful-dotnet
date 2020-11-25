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
        public EnigmaRotorSettings() => Rotors = new Dictionary<EnigmaRotorPosition, EnigmaRotor>
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
            get => Rotors[position];

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
            Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = (char)(((Rotors[EnigmaRotorPosition.Fastest].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');

            foreach (char notch in Rotors[EnigmaRotorPosition.Fastest].Notches)
            {
                if ((((Rotors[EnigmaRotorPosition.Fastest].CurrentSetting - 1 - 'A' + 26) % 26) + 'A') == notch)
                {
                    Rotors[EnigmaRotorPosition.Second].CurrentSetting = (char)(((Rotors[EnigmaRotorPosition.Second].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');

                    foreach (char notch2 in Rotors[EnigmaRotorPosition.Second].Notches)
                    {
                        if (Rotors[EnigmaRotorPosition.Second].CurrentSetting - 1 == notch2)
                        {
                            Rotors[EnigmaRotorPosition.Third].CurrentSetting = (char)(((Rotors[EnigmaRotorPosition.Third].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');
                            break;
                        }
                    }
                }

                // Doublestep the middle rotor when the right rotor is 2 past a notch and the middle is on a notch
                if ((((Rotors[EnigmaRotorPosition.Fastest].CurrentSetting - 2) % 'A') + 'A') == notch)
                {
                    foreach (char notch2 in Rotors[EnigmaRotorPosition.Second].Notches)
                    {
                        if (Rotors[EnigmaRotorPosition.Second].CurrentSetting == notch2)
                        {
                            Rotors[EnigmaRotorPosition.Second].CurrentSetting = (char)(((Rotors[EnigmaRotorPosition.Second].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');
                            Rotors[EnigmaRotorPosition.Third].CurrentSetting = (char)(((Rotors[EnigmaRotorPosition.Third].CurrentSetting + 1 - 'A' + 26) % 26) + 'A');
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
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in Rotors.Reverse().ToArray())
            {
                key.Append($"{position.Value.RingPosition:00}");
                key.Append(KeyDelimiter);
            }

            if (Rotors.Count > 0)
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

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in Rotors.Reverse().ToArray())
            {
                key.Append(position.Value.RotorNumber);
                key.Append(KeyDelimiter);
            }

            if (Rotors.Count > 0)
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
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in Rotors.Reverse().ToArray())
            {
                key.Append(position.Value.CurrentSetting);
                key.Append(KeyDelimiter);
            }

            if (Rotors.Count > 0)
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

            if (Rotors.Any())
            {
                foreach (EnigmaRotorPosition position in RotorPositions)
                {
                    if (!Rotors.ContainsKey(position))
                    {
                        continue;
                    }

                    if (availableRotors.Contains(Rotors[position].RotorNumber))
                    {
                        availableRotors.Remove(Rotors[position].RotorNumber);
                    }
                }
            }

            return availableRotors;
        }
    }
}