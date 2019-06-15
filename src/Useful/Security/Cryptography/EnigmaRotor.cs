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
        /// The current offset position of the rotor in relation to the letters.
        /// </summary>
        private int _currentSetting;

        private IList<int> _notches;

        /// <summary>
        /// The current offset position of the rotor's ring in relation to the letters.
        /// </summary>
        private int _ringPosition;

        /// <summary>
        /// The cipher for the wiring inside the rotor.
        /// </summary>
        private MonoAlphabeticSettings _wiring;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotor"/> class.
        /// </summary>
        /// <param name="rotorNumber">The rotor number.</param>
        public EnigmaRotor(EnigmaRotorNumber rotorNumber)
        {
            RotorNumber = rotorNumber;
            CanTurn = true;
            SetWiring();
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
        /// Gets or sets a value indicating whether this rotor advance positions.
        /// </summary>
        private bool CanTurn { get; set; }

        /// <summary>
        /// Advances the rotor one notch.
        /// </summary>
        public void AdvanceRotor()
        {
            // Don't advance the rotor if it can't turn
            if (!CanTurn)
            {
                return;
            }

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

        private void OnRotorAdvanced(bool isNotchHit, bool isDoubleStep)
        {
            RotorAdvanced?.Invoke(this, new EnigmaRotorAdvanceEventArgs(RotorNumber, isNotchHit, isDoubleStep));
        }

        private void SetWiring()
        {
            IDictionary<EnigmaRotorNumber, (string rotorWiring, string rotorNotches, bool canTurn)> wiring = new Dictionary<EnigmaRotorNumber, (string, string, bool)>()
            {
                { EnigmaRotorNumber.I, ("EKMFLGDQVZNTOWYHXUSPAIBRCJ", "Q", true) },
                { EnigmaRotorNumber.II, ("AJDKSIRUXBLHWTMCQGZNPYFVOE", "E", true) },
                { EnigmaRotorNumber.III, ("BDFHJLCPRTXVZNYEIWGAKMUSQO", "V", true) },
                { EnigmaRotorNumber.IV, ("ESOVPZJAYQUIRHXLNFTGKDCMWB", "J", true) },
                { EnigmaRotorNumber.V, ("VZBRGITYUPSDNHLXAWMJQOFECK", "Z", true) },
                { EnigmaRotorNumber.VI, ("JPGVOUMFYQBENHZRDKASXLICTW", "MZ", true) },
                { EnigmaRotorNumber.VII, ("NZJHGRCXMYSWBOUFAIVLPEKQDT", "MZ", true) },
                { EnigmaRotorNumber.VIII, ("FKQHTLXOCBJSPDZRAMEWNIUYGV", "MZ", true) },
                { EnigmaRotorNumber.Beta, ("LEYJVCNIXWPBQMDRTAKZGFUHOS", string.Empty, false) },
                { EnigmaRotorNumber.Gamma, ("FSOKANUERHMBTIYCWLQPZXVGJD", string.Empty, false) },
            };

            var (rotorWiring, rotorNotches, canTurn) = wiring[RotorNumber];

            //// Debug.Assert(rotorWiring.Length == Letters.Count, "Check for the correct number of letters");

            _wiring = new MonoAlphabeticSettings(CharacterSet, rotorWiring);

            // Set the notches
            _notches = new List<int>(rotorNotches.Length);
            foreach (char notch in rotorNotches)
            {
                _notches.Add(CharacterSet.IndexOf(notch));
            }

            CanTurn = canTurn;
        }
    }
}