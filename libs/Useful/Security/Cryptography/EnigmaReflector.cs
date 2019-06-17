// <copyright file="EnigmaReflector.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// An Enigma reflector.
    /// </summary>
    public class EnigmaReflector : IDisposable
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// States if this object been disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// The substitution of the rotor, effectively the wiring.
        /// </summary>
        private Reflector _wiring;

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
            IDictionary<EnigmaReflectorNumber, string> wiring = new Dictionary<EnigmaReflectorNumber, string>
            {
                { EnigmaReflectorNumber.B, "YRUHQSLDPXNGOKMIEBFZCWVJAT" },
                { EnigmaReflectorNumber.C, "FVPJIAOYEDRZXWGCTKUQSBNMHL" },
            };

            StringBuilder sb = new StringBuilder();
            List<char> unique = new List<char>();
            for (int i = 0; i < CharacterSet.Length; i++)
            {
                if (unique.Contains(CharacterSet[i])
                    || unique.Contains(wiring[ReflectorNumber][i]))
                {
                    continue;
                }

                if (CharacterSet[i] < wiring[ReflectorNumber][i])
                {
                    sb.Append(CharacterSet[i]);
                    sb.Append(wiring[ReflectorNumber][i]);
                }
                else
                {
                    sb.Append(wiring[ReflectorNumber][i]);
                    sb.Append(CharacterSet[i]);
                }

                sb.Append(" ");

                unique.Add(CharacterSet[i]);
                unique.Add(wiring[ReflectorNumber][i]);
            }

            sb.Remove(sb.Length - 1, 1);

            ReflectorSettings wiringSettings = new ReflectorSettings(CharacterSet, sb.ToString());
            _wiring?.Dispose();
            _wiring = new Reflector(wiringSettings);
        }
    }
}