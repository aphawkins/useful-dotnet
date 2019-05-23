// <copyright file="EnigmaRotor.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// An Enigma Rotor.
    /// </summary>
    public class EnigmaRotor : IDisposable
    {
        /// <summary>
        /// States if this cipher is symmetric i.e. two letters substitute to each other.
        /// </summary>
        private const bool IsRotorSymmetric = false;

        /// <summary>
        /// The current offset position of the rotor in relation to the letters.
        /// </summary>
        private int _currentSetting;

        /// <summary>
        /// State if the object has been disposed.
        /// </summary>
        private bool _isDisposed;

        private IList<int> _notches;

        /// <summary>
        /// The current offset position of the rotor's ring in relation to the letters.
        /// </summary>
        private int _ringPosition;

        /// <summary>
        /// The cipher for the wiring inside the rotor.
        /// </summary>
        private MonoAlphabeticCipher _wiring;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaRotor"/> class.
        /// </summary>
        /// <param name="rotorNumber">The rotor number.</param>
        public EnigmaRotor(EnigmaRotorNumber rotorNumber)
        {
            RotorNumber = rotorNumber;
            CanTurn = true;
            SetWiring();
            if (rotorNumber != EnigmaRotorNumber.None)
            {
                RingPosition = 'A';
                CurrentSetting = 'A';
            }
        }

        /// <summary>
        /// Raised when the rotor is advanced
        /// </summary>
        public event EventHandler<EnigmaRotorAdvanceEventArgs> RotorAdvanced;

        ///// <summary>
        ///// Raised when the rotor is reversed
        ///// </summary>
        // public event EventHandler<EnigmaRotorAdvanceEventArgs> RotorReversed;

        /// <summary>
        /// Gets or sets the current letter the rotor is set to.
        /// </summary>
        public char CurrentSetting
        {
            get
            {
                return Letters[_currentSetting];
            }

            set
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
                }

                if (!Letters.Contains(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _currentSetting = Letters.IndexOf(value);
            }
        }

        /// <summary>
        /// Gets the letters available to this rotor.
        /// </summary>
        public IList<char> Letters { get; private set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /// <summary>
        /// Gets or sets the current letter the rotor's ring is set to.
        /// </summary>
        public char RingPosition
        {
            get
            {
                return Letters[_ringPosition];
            }

            set
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
                }

                if (!Letters.Contains(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _ringPosition = Letters.IndexOf(value);
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
            if (_isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
            }

            // Don't advance the rotor if it can't turn
            if (!CanTurn)
            {
                return;
            }

            _currentSetting++;
            _currentSetting %= Letters.Count;

            bool isNotchHit = false;
            bool isDoubleStep = false;

            foreach (int notch in _notches)
            {
                if (_currentSetting == ((notch + 1) % Letters.Count))
                {
                    isNotchHit = true;
                    break;
                }
                else if (_currentSetting == ((notch + 2) % Letters.Count))
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
            if (_isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
            }

            // Add the offset the current position
            int currentPosition = Letters.IndexOf(letter);
            int newLet = (currentPosition + _currentSetting - _ringPosition + Letters.Count) % Letters.Count;
            if (newLet < 0 || newLet >= Letters.Count)
            {
                throw new IndexOutOfRangeException();
            }

            char newLetter = Letters[newLet];

            newLetter = _wiring.Decrypt(newLetter);

            // Undo offset the current position
            currentPosition = Letters.IndexOf(newLetter);
            newLet = (currentPosition - _currentSetting + _ringPosition + Letters.Count) % Letters.Count;
            if (newLet < 0 || newLet >= Letters.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return Letters[newLet];
        }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The letter this rotor encodes to going forward through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter.</returns>
        public char Forward(char letter)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
            }

            // Add the offset the current position
            int currentPosition = Letters.IndexOf(letter);
            int newLet = (currentPosition + _currentSetting - _ringPosition + Letters.Count) % Letters.Count;
            if (newLet < 0 || newLet >= Letters.Count)
            {
                throw new IndexOutOfRangeException();
            }

            char newLetter = Letters[newLet];

            newLetter = _wiring.Encrypt(newLetter);

            // Undo offset the current position
            currentPosition = Letters.IndexOf(newLetter);
            newLet = (currentPosition - _currentSetting + _ringPosition + Letters.Count) % Letters.Count;
            if (newLet < 0 || newLet >= Letters.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return Letters[newLet];
        }
        /////// <summary>
        /////// The previous rotor has advanced.
        /////// </summary>
        /////// <param name="e">The event arguments.</param>
        //// public void PreviousRotorAdvanced(EnigmaRotorAdvanceEventArgs e)
        //// {
        ////    if (e.IsNotchHit)
        ////    {
        ////        AdvanceRotor();
        ////    }

        ////    if (e.IsDoubleStep && _notches.Contains(_currentSetting))
        ////    {
        ////        AdvanceRotor();
        ////    }
        //// }

        /////// <summary>
        /////// Reverses the rotor one notch.
        /////// </summary>
        //// public void ReverseRotor()
        //// {
        ////    Contract.Requires(this.Letters.Count != 0);

        //// if (this.isDisposed)
        ////    {
        ////        throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
        ////    }

        //// // Don't reverse the rotor if it can't turn
        ////    if (!this.CanTurn)
        ////    {
        ////        return;
        ////    }

        //// int setting = (this.currentSetting - 1 + this.Letters.Count) % this.Letters.Count;

        //// if (0 > setting) throw new IndexOutOfRangeException();

        //// this.SetCurrentSetting(setting);

        //// this.OnRotorReversed();

        //// if (0 > this.currentSetting || this.currentSetting >= this.Letters.Count) throw new IndexOutOfRangeException();

        //// foreach (int notch in this.notches)
        ////    {
        ////        if (this.currentSetting == (notch - 1 + this.Letters.Count) % this.Letters.Count)
        ////        {
        ////            this.OnRotorReversedNotchHit();
        ////            break;
        ////        }
        ////    }
        //// }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            // A call to Dispose(false) should only clean up native resources.
            // A call to Dispose(true) should clean up both managed and native resources.
            if (disposing)
            {
                // Dispose managed resources
                if (_wiring != null)
                {
                    _wiring.Dispose();
                }
            }

            // Free native resources
            _isDisposed = true;
        }

        private void OnRotorAdvanced(bool isNotchHit, bool isDoubleStep)
        {
            RotorAdvanced?.Invoke(this, new EnigmaRotorAdvanceEventArgs(RotorNumber, isNotchHit, isDoubleStep));
        }

        private void SetWiring()
        {
            IDictionary<EnigmaRotorNumber, (string rotorWiring, string rotorNotches, bool canTurn)> wiring = new Dictionary<EnigmaRotorNumber, (string, string, bool)>()
            {
                { EnigmaRotorNumber.None, (string.Empty, string.Empty, false) },
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

            IDictionary<char, char> rotorPairs = new Dictionary<char, char>();
            for (int i = 0; i < Letters.Count; i++)
            {
                rotorPairs.Add(Letters[i], rotorWiring[i]);
            }

            MonoAlphabeticSettings wiringSettings = new MonoAlphabeticSettings(Letters, rotorPairs, IsRotorSymmetric);
            _wiring?.Dispose();
            _wiring = new MonoAlphabeticCipher(wiringSettings);

            // Set the notches
            _notches = new List<int>(rotorNotches.Length);
            foreach (char notch in rotorNotches)
            {
                _notches.Add(Letters.IndexOf(notch));
            }

            CanTurn = canTurn;
        }

        // private void OnRotorReversed()
        // {
        //    if (this.RotorReversed != null)
        //    {
        //        this.RotorReversed(this, new EnigmaRotorAdvanceEventArgs(this.RotorNumber, false));
        //    }
        // }
    }
}