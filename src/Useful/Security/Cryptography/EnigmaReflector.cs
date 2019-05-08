//-----------------------------------------------------------------------
// <copyright file="EnigmaReflector.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>An Enigma reflector.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Security.Cryptography;

    /// <summary>
    /// An Enigma reflector.
    /// </summary>
    public class EnigmaReflector : IDisposable
    {
        #region Fields
        /// <summary>
        /// Defines if the reflector is symmetric i.e. if one letter substitutes to another letter and vice versa.
        /// </summary>
        private const bool IsReflectorSymmetric = false;

        /// <summary>
        /// The substitution of the rotor, effectively the wiring.
        /// </summary>
        private MonoAlphabeticTransform wiring;

        /// <summary>
        /// The settings for the wiring.
        /// </summary>
        private MonoAlphabeticSettings wiringSettings;

        /// <summary>
        /// States if this object been disposed.
        /// </summary>
        private bool isDisposed;
        #endregion

        #region ctor
        /// <summary>
        /// Initializes a new instance of the EnigmaReflector class.
        /// </summary>
        /// <param name="reflectorNumber"></param>
        private EnigmaReflector(EnigmaReflectorNumber reflectorNumber)
        {
            this.ReflectorNumber = reflectorNumber;
            this.SetWiring();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the designation of this reflector.
        /// </summary>
        public EnigmaReflectorNumber ReflectorNumber { get; private set; }
        #endregion

        #region Methods
        public static EnigmaReflector Create(EnigmaReflectorNumber reflectorNumber)
        {
            EnigmaReflector reflector = new EnigmaReflector(reflectorNumber);
            return reflector;
        }


        /// <summary>
        /// The letter this reflector encodes to going through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter</returns>
        public char Reflect(char letter)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaReflector).ToString());
            }

            char newLetter = this.wiring.Encipher(letter);

            return newLetter;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the objects are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Retrieves a value that indicates the hash code value for this object.
        /// </summary>
        /// <returns>The hash code value for this object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            // A call to Dispose(false) should only clean up native resources. 
            // A call to Dispose(true) should clean up both managed and native resources.
            if (disposing)
            {
                // Dispose managed resources
                if (this.wiring != null)
                {
                    this.wiring.Dispose();
                }
            }

            // Free native resources
            this.isDisposed = true;
        }

        /// <summary>
        /// Sets the wiring based on the reflector.
        /// </summary>
        private void SetWiring()
        {
            Contract.Requires(Enum.IsDefined(typeof(EnigmaReflectorNumber), this.ReflectorNumber));

            char[] reflectorWiring;

            switch (this.ReflectorNumber)
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

            Contract.Assert(Letters.EnglishAlphabetUppercase.Count == reflectorWiring.Length);

            for (int i = 0; i < Letters.EnglishAlphabetUppercase.Count; i++)
            {
                reflectorPairs.Add(Letters.EnglishAlphabetUppercase[i], reflectorWiring[i]);
            }

            byte[] wiringKey = MonoAlphabeticSettings.BuildKey(new Collection<char>(reflectorWiring), reflectorPairs, IsReflectorSymmetric);
            byte[] wiringIV = MonoAlphabeticSettings.BuildIV();
            this.wiringSettings = new MonoAlphabeticSettings(wiringKey, wiringIV);
            CipherTransformMode transformMode = CipherTransformMode.Encrypt;
            Contract.Assert(Enum.IsDefined(typeof(CipherTransformMode), transformMode));
            this.wiring = new MonoAlphabeticTransform(this.wiringSettings.Key, this.wiringSettings.IV, transformMode);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.wiring != null);
            Contract.Invariant(this.wiringSettings != null);
            Contract.Invariant(Enum.IsDefined(typeof(EnigmaReflectorNumber), this.ReflectorNumber));
        }

        internal static EnigmaReflector GetDefault(EnigmaModel model)
        {
            switch (model)
            {
                case EnigmaModel.Military:
                    return EnigmaReflector.Create(EnigmaReflectorNumber.B);
                case EnigmaModel.M3:
                case EnigmaModel.M4:
                    return EnigmaReflector.Create(EnigmaReflectorNumber.BThin);
                default:
                    throw new CryptographicException("Unknown Enigma model.");
            }
        }

        internal static EnigmaReflector GetRandom(EnigmaModel model)
        {
            Random rnd = new Random();

            List<EnigmaReflectorNumber> reflectors = GetAllowed(model);

            int nextRandomNumber = rnd.Next(0, reflectors.Count);

            return EnigmaReflector.Create(reflectors[nextRandomNumber]);
        }

        internal static List<EnigmaReflectorNumber> GetAllowed(EnigmaModel model)
        {
            switch (model)
            {
                case (EnigmaModel.Military):
                case (EnigmaModel.M3):
                    {
                        return new List<EnigmaReflectorNumber>(2) {
                            EnigmaReflectorNumber.B,
                            EnigmaReflectorNumber.C
                            };
                    }

                case (EnigmaModel.M4):
                    {
                        return new List<EnigmaReflectorNumber>(2) {
                            EnigmaReflectorNumber.BThin,
                            EnigmaReflectorNumber.CThin
                            };
                    }
                default:
                    {
                        throw new CryptographicException("Unknown Enigma model.");
                    }
            }
        }

        #endregion
    }
}
