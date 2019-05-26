// <copyright file="EnigmaReflector.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Cryptography;

    /// <summary>
    /// An Enigma reflector.
    /// </summary>
    public class EnigmaReflector : IDisposable
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Defines if the reflector is symmetric i.e. if one letter substitutes to another letter and vice versa.
        /// </summary>
        private const bool IsReflectorSymmetric = false;

        /// <summary>
        /// States if this object been disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// The substitution of the rotor, effectively the wiring.
        /// </summary>
        private MonoAlphabeticCipher _wiring;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaReflector"/> class.
        /// </summary>
        /// <param name="reflectorNumber">The reflector number.</param>
        public EnigmaReflector(EnigmaReflectorNumber reflectorNumber)
        {
            ReflectorNumber = reflectorNumber;
            SetWiring();
        }

        /// <summary>
        /// Gets the designation of this reflector.
        /// </summary>
        public EnigmaReflectorNumber ReflectorNumber { get; private set; }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The letter this reflector encodes to going through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter.</returns>
        public char Reflect(char letter)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaReflector).ToString());
            }

            char newLetter = _wiring.Encrypt(letter);

            return newLetter;
        }

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
                _wiring?.Dispose();
            }

            // Free native resources
            _isDisposed = true;
        }

        /// <summary>
        /// Sets the wiring based on the reflector.
        /// </summary>
        private void SetWiring()
        {
            char[] reflectorWiring;

            switch (ReflectorNumber)
            {
                case EnigmaReflectorNumber.A:
                    {
                        reflectorWiring = @"EJMZALYXVBWFCRQUONTSPIKHGD".ToCharArray();
                        break;
                    }

                case EnigmaReflectorNumber.B:
                    {
                        reflectorWiring = @"YRUHQSLDPXNGOKMIEBFZCWVJAT".ToCharArray();
                        break;
                    }

                case EnigmaReflectorNumber.C:
                    {
                        reflectorWiring = @"FVPJIAOYEDRZXWGCTKUQSBNMHL".ToCharArray();
                        break;
                    }

                case EnigmaReflectorNumber.BThin:
                    {
                        reflectorWiring = @"ENKQAUYWJICOPBLMDXZVFTHRGS".ToCharArray();
                        break;
                    }

                case EnigmaReflectorNumber.CThin:
                    {
                        reflectorWiring = @"RDOBJNTKVEHMLFCWZAXGYIPSUQ".ToCharArray();
                        break;
                    }

                default:
                    {
                        throw new CryptographicException("Unknown reflector.");
                    }
            }

            Dictionary<char, char> reflectorPairs = new Dictionary<char, char>();

            Debug.Assert(CharacterSet.Length == reflectorWiring.Length, "Wiring length should equal letter count.");

            for (int i = 0; i < CharacterSet.Length; i++)
            {
                reflectorPairs.Add(CharacterSet[i], reflectorWiring[i]);
            }

            MonoAlphabeticSettings wiringSettings = new MonoAlphabeticSettings(new List<char>(CharacterSet), reflectorPairs, IsReflectorSymmetric);
            _wiring?.Dispose();
            _wiring = new MonoAlphabeticCipher(wiringSettings);
        }
    }
}