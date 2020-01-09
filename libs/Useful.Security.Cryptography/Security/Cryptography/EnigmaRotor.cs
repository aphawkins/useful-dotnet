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
    public class EnigmaRotor
    {
        /// <summary>
        /// Gets the letters available to this rotor.
        /// </summary>
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// The cipher for the wiring inside the rotor.
        /// </summary>
        private readonly MonoAlphabeticSettings _wiring;

        /// <summary>
        /// The current offset position of the rotor in relation to the letters.
        /// </summary>
        private int _currentSetting;

        /// <summary>
        /// The current offset position of the rotor's ring in relation to the letters.
        /// </summary>
        private int _ringPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotor"/> class.
        /// </summary>
        /// <param name="rotorNumber">The rotor number.</param>
        public EnigmaRotor(EnigmaRotorNumber rotorNumber)
        {
            RotorNumber = rotorNumber;
            (_wiring, Notches) = GetWiring(rotorNumber);
            RingPosition = 1;
            CurrentSetting = 'A';
        }

        /// <summary>
        /// Gets or sets the current letter the rotor is set to.
        /// </summary>
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

        /// <summary>
        /// Gets the notches.
        /// </summary>
        public string Notches
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the current letter the rotor's ring is set to.
        /// </summary>
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

        /// <summary>
        /// Gets the designation of this rotor.
        /// </summary>
        public EnigmaRotorNumber RotorNumber { get; private set; }

        /// <summary>
        /// The letter this rotor encodes to going backwards through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter.</returns>
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

        /// <summary>
        /// The letter this rotor encodes to going forward through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter.</returns>
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

        private static (MonoAlphabeticSettings wiringSettings, string notches) GetWiring(EnigmaRotorNumber rotorNumber)
        {
            IDictionary<EnigmaRotorNumber, (string rotorWiring, string notches)> wiring = new Dictionary<EnigmaRotorNumber, (string, string)>()
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

            MonoAlphabeticSettings wiringSettings = new MonoAlphabeticSettings(CharacterSet, rotorWiring);

            return (wiringSettings, notches);
        }
    }
}