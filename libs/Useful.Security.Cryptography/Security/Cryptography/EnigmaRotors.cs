// <copyright file="EnigmaRotors.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Enigma rotor settings.
    /// </summary>
    public sealed class EnigmaRotors : IEnigmaRotors
    {
        private IReadOnlyDictionary<EnigmaRotorPosition, IEnigmaRotor> _rotors = new Dictionary<EnigmaRotorPosition, IEnigmaRotor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotors"/> class.
        /// </summary>
        public EnigmaRotors() => _rotors = new Dictionary<EnigmaRotorPosition, IEnigmaRotor>
            {
                { EnigmaRotorPosition.Fastest, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.I } },
                { EnigmaRotorPosition.Second, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.II } },
                { EnigmaRotorPosition.Third, new EnigmaRotor() { RotorNumber = EnigmaRotorNumber.III } },
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotors"/> class.
        /// </summary>
        /// <param name="rotors">The rotors.</param>
        public EnigmaRotors(IReadOnlyDictionary<EnigmaRotorPosition, IEnigmaRotor> rotors) => Rotors = rotors;

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
        public static IList<EnigmaRotorNumber> RotorSet => new List<EnigmaRotorNumber>()
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

        /// <inheritdoc />
        public IReadOnlyDictionary<EnigmaRotorPosition, IEnigmaRotor> Rotors
        {
            get => _rotors;
            init
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

        /// <inheritdoc />
        public IEnigmaRotor this[EnigmaRotorPosition position] => _rotors[position];

        /// <inheritdoc />
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
    }
}