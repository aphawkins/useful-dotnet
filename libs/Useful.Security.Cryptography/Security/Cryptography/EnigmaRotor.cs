// <copyright file="EnigmaRotor.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An Enigma Rotor.
    /// </summary>
    public sealed class EnigmaRotor : IEnigmaRotor
    {
        /// <summary>
        /// Gets the letters available to this rotor.
        /// </summary>
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// The cipher for the wiring inside the rotor.
        /// </summary>
        private MonoAlphabeticSettings _wiring;

        /// <summary>
        /// The current offset position of the rotor in relation to the letters.
        /// </summary>
        private int _currentSetting;

        /// <summary>
        /// The current offset position of the rotor's ring in relation to the letters.
        /// </summary>
        private int _ringPosition;

        private EnigmaRotorNumber _rotorNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotor"/> class.
        /// </summary>
        public EnigmaRotor()
        {
            RotorNumber = EnigmaRotorNumber.I;
            (_wiring, Notches) = GetWiring(RotorNumber);
            RingPosition = 1;
            CurrentSetting = 'A';
        }

        /// <inheritdoc />
        public char CurrentSetting
        {
            get => CharacterSet[_currentSetting];

            set
            {
                if (CharacterSet.IndexOf(value) < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _currentSetting = CharacterSet.IndexOf(value);
            }
        }

        /// <inheritdoc />
        public string Notches
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public int RingPosition
        {
            get => _ringPosition;

            set
            {
                if (value < 1 || value > CharacterSet.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _ringPosition = value;
            }
        }

        /// <inheritdoc />
        public EnigmaRotorNumber RotorNumber
        {
            get => _rotorNumber;
            set
            {
                _rotorNumber = value;
                (_wiring, Notches) = GetWiring(_rotorNumber);
                RingPosition = 1;
                CurrentSetting = 'A';
            }
        }

        /// <inheritdoc />
        public char Backward(char letter)
        {
            // Add the offset the current position
            int currentPosition = CharacterSet.IndexOf(letter);
            int newLet = (currentPosition + _currentSetting - _ringPosition + 1 + CharacterSet.Length) % CharacterSet.Length;
            if (newLet < 0 || newLet >= CharacterSet.Length)
            {
                throw new IndexOutOfRangeException();
            }

            char newLetter = CharacterSet[newLet];

            newLetter = _wiring.Reverse(newLetter);

            // Undo offset the current position
            currentPosition = CharacterSet.IndexOf(newLetter);
            newLet = (currentPosition - _currentSetting + _ringPosition - 1 + CharacterSet.Length) % CharacterSet.Length;
            if (newLet < 0 || newLet >= CharacterSet.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return CharacterSet[newLet];
        }

        /// <inheritdoc />
        public char Forward(char letter)
        {
            // Add the offset the current position
            int currentPosition = CharacterSet.IndexOf(letter);
            int newLet = (currentPosition + _currentSetting - _ringPosition + 1 + CharacterSet.Length) % CharacterSet.Length;
            char newLetter = CharacterSet[newLet];

            newLetter = _wiring[newLetter];

            // Undo offset the current position
            currentPosition = CharacterSet.IndexOf(newLetter);
            newLet = (currentPosition - _currentSetting + _ringPosition - 1 + CharacterSet.Length) % CharacterSet.Length;

            return CharacterSet[newLet];
        }

        private static (MonoAlphabeticSettings WiringSettings, string Notches) GetWiring(EnigmaRotorNumber rotorNumber)
        {
            IDictionary<EnigmaRotorNumber, (string RotorWiring, string Notches)> wiring = new Dictionary<EnigmaRotorNumber, (string, string)>()
            {
                { EnigmaRotorNumber.I, ("EKMFLGDQVZNTOWYHXUSPAIBRCJ", "Q") },
                { EnigmaRotorNumber.II, ("AJDKSIRUXBLHWTMCQGZNPYFVOE", "E") },
                { EnigmaRotorNumber.III, ("BDFHJLCPRTXVZNYEIWGAKMUSQO", "V") },
                { EnigmaRotorNumber.IV, ("ESOVPZJAYQUIRHXLNFTGKDCMWB", "J") },
                { EnigmaRotorNumber.V, ("VZBRGITYUPSDNHLXAWMJQOFECK", "Z") },
                { EnigmaRotorNumber.VI, ("JPGVOUMFYQBENHZRDKASXLICTW", "MZ") },
                { EnigmaRotorNumber.VII, ("NZJHGRCXMYSWBOUFAIVLPEKQDT", "MZ") },
                { EnigmaRotorNumber.VIII, ("FKQHTLXOCBJSPDZRAMEWNIUYGV", "MZ") },
            };

            (string rotorWiring, string notches) = wiring[rotorNumber];

            MonoAlphabeticSettings wiringSettings = new() { CharacterSet = CharacterSet, Substitutions = rotorWiring };

            return (wiringSettings, notches);
        }
    }
}