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

        private readonly IList<int> _notches;

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
            (_wiring, _notches) = GetWiring(rotorNumber);
            RingPosition = 1;
            CurrentSetting = 'A';
        }

        /// <summary>
        /// Raised when the rotor is advanced
        /// </summary>
        public event EventHandler<EnigmaRotorAdvanceEventArgs> RotorAdvanced;

        /// <summary>
        /// Gets or sets the current letter the rotor is set to.
        /// </summary>
        public char CurrentSetting
        {
            get
            {
                return CharacterSet[_currentSetting];
            }

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
        /// Gets or sets the current letter the rotor's ring is set to.
        /// </summary>
        public int RingPosition
        {
            get
            {
                return _ringPosition;
            }

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
        /// Advances the rotor one notch.
        /// </summary>
        public void AdvanceRotor()
        {
            _currentSetting++;
            _currentSetting %= CharacterSet.Length;

            bool isNotchHit = false;
            bool isDoubleStep = false;

            foreach (int notch in _notches)
            {
                if (_currentSetting == ((notch + 1) % CharacterSet.Length))
                {
                    isNotchHit = true;
                    break;
                }
                else if (_currentSetting == ((notch + 2) % CharacterSet.Length))
                {
                    isDoubleStep = true;
                    break;
                }
            }

            OnRotorAdvanced(isNotchHit, isDoubleStep);
        }

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

        private static (MonoAlphabeticSettings, IList<int>) GetWiring(EnigmaRotorNumber rotorNumber)
        {
            IDictionary<EnigmaRotorNumber, (string rotorWiring, string rotorNotches)> wiring = new Dictionary<EnigmaRotorNumber, (string, string)>()
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

            var (rotorWiring, rotorNotches) = wiring[rotorNumber];

            MonoAlphabeticSettings wiringSettings = new MonoAlphabeticSettings(CharacterSet, rotorWiring);

            // Set the notches
            IList<int> notches = new List<int>(rotorNotches.Length);
            foreach (char notch in rotorNotches)
            {
                notches.Add(CharacterSet.IndexOf(notch));
            }

            return (wiringSettings, notches);
        }

        private void OnRotorAdvanced(bool isNotchHit, bool isDoubleStep)
        {
            RotorAdvanced?.Invoke(this, new EnigmaRotorAdvanceEventArgs(RotorNumber, isNotchHit, isDoubleStep));
        }
    }
}