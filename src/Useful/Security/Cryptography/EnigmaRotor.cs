//-----------------------------------------------------------------------
// <copyright file="EnigmaRotor.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>An Enigma Rotor.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Security.Cryptography;
    using System.Linq;

    /// <summary>
    /// An Enigma Rotor.
    /// </summary>
    public class EnigmaRotor : IDisposable
    {
        #region Fields
        /// <summary>
        /// States if this cipher is symmetric i.e. two letters substitute to each other.
        /// </summary>
        private const bool IsRotorSymmetric = false;

        /// <summary>
        /// The letters available to this rotor.
        /// </summary>
        private static readonly Collection<char> rotorLetters = new Collection<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());

        /// <summary>
        /// The cipher for the wiring inside the rotor.
        /// </summary>
        private MonoAlphabeticTransform wiring;

        /// <summary>
        /// The wiring inside the rotor.
        /// </summary>
        private MonoAlphabeticSettings wiringSettings;

        /// <summary>
        /// The current offset position of the rotor in relation to the letters.
        /// </summary>
        private int currentSetting;

        /// <summary>
        /// The current offset position of the rotor's ring in relation to the letters.
        /// </summary>
        private int ringPosition;

        /// <summary>
        /// State if the object has been disposed.
        /// </summary>
        private bool isDisposed;

        private List<int> notches;
        #endregion

        #region ctor
        /// <summary>
        /// Initializes a new instance of the EnigmaRotor class.
        /// </summary>
        /// <param name="rotorNumber"></param>
        private EnigmaRotor(EnigmaRotorNumber rotorNumber)
        {
            this.RotorNumber = rotorNumber;
            this.Letters = GetAllowedLetters(this.RotorNumber);
            this.CanTurn = true;
            this.SetWiring();
            if (rotorNumber != EnigmaRotorNumber.None)
            {
                this.RingPosition = 'A';
                this.CurrentSetting = 'A';
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Raised when the rotor is advanced
        /// </summary>
        public event EventHandler<EnigmaRotorAdvanceEventArgs> RotorAdvanced;

        ///// <summary>
        ///// Raised when the rotor is reversed
        ///// </summary>
        //public event EventHandler<EnigmaRotorAdvanceEventArgs> RotorReversed;
        #endregion

        #region Properties
        /// <summary>
        /// The letters available to this rotor.
        /// </summary>
        public Collection<char> Letters { get; private set; }

        /// <summary>
        /// Gets the designation of this rotor.
        /// </summary>
        public EnigmaRotorNumber RotorNumber { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this rotor advance positions.
        /// </summary>
        private bool CanTurn { get; set; }

        /// <summary>
        /// Gets the current letter the rotor is set to.
        /// </summary>
        public char CurrentSetting
        {
            get
            {
                Contract.Requires(this.RotorNumber != EnigmaRotorNumber.None);

                return this.Letters[this.currentSetting];
            }
            set
            {
                Contract.Requires(this.Letters.Contains(value));

                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
                }

                this.currentSetting = this.Letters.IndexOf(value);
            }
        }

        /// <summary>
        /// Gets the current letter the rotor's ring is set to.
        /// </summary>
        public char RingPosition
        {
            get
            {
                Contract.Requires(this.RotorNumber != EnigmaRotorNumber.None);

                return this.Letters[this.ringPosition];
            }
            set
            {
                Contract.Requires(this.Letters.Contains(value));

                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
                }

                this.ringPosition = this.Letters.IndexOf(value);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorNumber"></param>
        /// <returns></returns>
        public static EnigmaRotor Create(EnigmaRotorNumber rotorNumber)
        {
            return new EnigmaRotor(rotorNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorNumber"></param>
        /// <returns></returns>
        public static Collection<char> GetAllowedLetters(EnigmaRotorNumber rotorNumber)
        {
            Contract.Requires(Enum.IsDefined(typeof(EnigmaRotorNumber), rotorNumber));
            Contract.Ensures(Contract.Result<Collection<char>>() != null);

            switch (rotorNumber)
            {
                case EnigmaRotorNumber.None:
                    {
                        return new Collection<char>();
                    }

                case EnigmaRotorNumber.One:
                case EnigmaRotorNumber.Two:
                case EnigmaRotorNumber.Three:
                case EnigmaRotorNumber.Four:
                case EnigmaRotorNumber.Five:
                case EnigmaRotorNumber.Beta:
                case EnigmaRotorNumber.Gamma:
                    {
                        return EnigmaRotor.rotorLetters;
                    }

                default:
                    {
                        throw new CryptographicException("Unknown rotor");
                    }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorOrder"></param>
        /// <returns></returns>
        public static EnigmaRotor GetRandom(EnigmaRotorNumber rotorNumber)
        {
            Random rnd = new Random();
            Collection<char> letters = EnigmaRotor.GetAllowedLetters(rotorNumber);

            int index1 = rnd.Next(0, letters.Count);
            int index2 = rnd.Next(0, letters.Count);

            EnigmaRotor rotor = EnigmaRotor.Create(rotorNumber);
            rotor.ringPosition = letters[index1];
            rotor.currentSetting = letters[index2];

            return rotor;
        }

        /// <summary>
        /// The letter this rotor encodes to going forward through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter</returns>
        public char Forward(char letter)
        {
            Contract.Requires(this.Letters.Count != 0);

            if (this.isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
            }

            // Add the offset the current position
            int currentPosition = this.Letters.IndexOf(letter);
            int newLet = (currentPosition + this.currentSetting - this.ringPosition + this.Letters.Count) % this.Letters.Count;
            if (0 > newLet || newLet >= this.Letters.Count) throw new IndexOutOfRangeException();
            char newLetter = this.Letters[newLet];

            newLetter = this.wiring.Encipher(newLetter);

            // Undo offset the current position
            currentPosition = this.Letters.IndexOf(newLetter);
            newLet = (currentPosition - this.currentSetting + this.ringPosition + this.Letters.Count) % this.Letters.Count;
            if (0 > newLet || newLet >= this.Letters.Count) throw new IndexOutOfRangeException();
            Contract.Assert(newLet >= 0);
            return this.Letters[newLet];
        }

        /// <summary>
        /// The letter this rotor encodes to going backwards through it.
        /// </summary>
        /// <param name="letter">The letter to transform.</param>
        /// <returns>The transformed letter</returns>
        public char Backward(char letter)
        {
            Contract.Requires(this.Letters.Count != 0);

            if (this.isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
            }

            // Add the offset the current position
            int currentPosition = this.Letters.IndexOf(letter);
            int newLet = (currentPosition + this.currentSetting - this.ringPosition + this.Letters.Count) % this.Letters.Count;
            if (0 > newLet || newLet >= this.Letters.Count) throw new IndexOutOfRangeException();
            char newLetter = this.Letters[newLet];

            newLetter = this.wiring.Decipher(newLetter);

            // Undo offset the current position
            currentPosition = this.Letters.IndexOf(newLetter);
            newLet = (currentPosition - this.currentSetting + this.ringPosition + this.Letters.Count) % this.Letters.Count;
            if (0 > newLet || newLet >= this.Letters.Count) throw new IndexOutOfRangeException();
            return this.Letters[newLet];
        }

        /// <summary>
        /// Advances the rotor one notch.
        /// </summary>
        public void AdvanceRotor()
        {
            Contract.Requires(this.Letters.Count > 0);

            if (this.isDisposed)
            {
                throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
            }

            // Don't advance the rotor if it can't turn
            if (!this.CanTurn)
            {
                return;
            }

            int setting = (this.currentSetting + 1) % this.Letters.Count;

            Contract.Assert(setting >= 0);

            this.currentSetting = setting;

            bool isNotchHit = false;
            bool isDoubleStep = false;

            foreach (int notch in this.notches)
            {
                if (this.currentSetting == ((notch + 1) % this.Letters.Count))
                {
                    isNotchHit = true;
                    break;
                }
                else if (this.currentSetting == ((notch + 2) % this.Letters.Count))
                {
                    isDoubleStep = true;
                    break;
                }
            }

            this.OnRotorAdvanced(isNotchHit, isDoubleStep);
        }

        public void PreviousRotorAdvanced(object sender, EnigmaRotorAdvanceEventArgs e)
        {
            if (e.IsNotchHit)
            {
                this.AdvanceRotor();
            }
            if (e.IsDoubleStep && this.notches.Contains(this.currentSetting))
            {
                this.AdvanceRotor();
            }
        }

        ///// <summary>
        ///// Reverses the rotor one notch.
        ///// </summary>
        //public void ReverseRotor()
        //{
        //    Contract.Requires(this.Letters.Count != 0);

        //    if (this.isDisposed)
        //    {
        //        throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
        //    }

        //    // Don't reverse the rotor if it can't turn
        //    if (!this.CanTurn)
        //    {
        //        return;
        //    }

        //    int setting = (this.currentSetting - 1 + this.Letters.Count) % this.Letters.Count;

        //    if (0 > setting) throw new IndexOutOfRangeException();

        //    this.SetCurrentSetting(setting);

        //    this.OnRotorReversed();

        //    if (0 > this.currentSetting || this.currentSetting >= this.Letters.Count) throw new IndexOutOfRangeException();

        //    foreach (int notch in this.notches)
        //    {
        //        if (this.currentSetting == (notch - 1 + this.Letters.Count) % this.Letters.Count)
        //        {
        //            this.OnRotorReversedNotchHit();
        //            break;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Sets the current letter the rotor is set to.
        ///// </summary>
        ///// <param name="setting">The letter the rotor is positioned at.</param>
        //public void SetCurrentSetting(char setting)
        //{
        //    Contract.Requires(this.Letters.Contains(setting));

        //    //if (!this.Letters.Contains(setting))
        //    //{
        //    //    throw new ArgumentException("This setting is not allowed.");
        //    //}

        //    if (this.isDisposed)
        //    {
        //        throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
        //    }

        //    if (!this.Letters.Contains(setting))
        //    {
        //        throw new CryptographicException();
        //    }

        //    Contract.Assert(this.Letters.IndexOf(setting) >= 0);
        //    this.SetCurrentSetting(this.Letters.IndexOf(setting));
        //}

        ///// <summary>
        ///// Sets the current letter the rotor's ring is set to.
        ///// </summary>
        ///// <param name="setting">The letter the rotor's ring is positioned at.</param>
        //public void SetRingPosition(char setting)
        //{
        //    Contract.Requires(this.Letters.IndexOf(setting) >= 0);

        //    if (this.isDisposed)
        //    {
        //        throw new ObjectDisposedException(typeof(EnigmaRotor).ToString());
        //    }

        //    if (!this.Letters.Contains(setting))
        //    {
        //        throw new CryptographicException();
        //    }

        //    Contract.Assert(this.Letters.IndexOf(setting) >= 0);
        //    this.SetRingPosition(this.Letters.IndexOf(setting));
        //}

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

        ///// <summary>
        ///// Sets the current position the rotor is set to.
        ///// </summary>
        ///// <param name="setting">The current position the rotor is set to.</param>
        //private void SetCurrentSetting(int setting)
        //{
        //    Contract.Requires(setting >= 0);
        //    Contract.Requires(setting < this.Letters.Count);

        //    this.currentSetting = setting;

        //    Contract.Assume(setting < this.Letters.Count);

        //    this.CurrentSetting = this.Letters[setting];
        //}

        ///// <summary>
        ///// Sets the current position the rotor's ring is set to.
        ///// </summary>
        ///// <param name="setting">The current position the rotor's ring is set to.</param>
        //private void SetRingPosition(int setting)
        //{
        //    Contract.Requires(setting >= 0);
        //    Contract.Requires(setting < this.Letters.Count);

        //    this.ringPosition = setting;

        //    Contract.Assume(setting < this.Letters.Count);

        //    this.RingPosition = this.Letters[setting];
        //}

        private void SetWiring()
        {
            Contract.Requires(this.Letters != null);

            char[] rotorWiring;
            char[] rotorNotches;

            switch (this.RotorNumber)
            {
                case EnigmaRotorNumber.None:
                    {
                        rotorWiring = string.Empty.ToCharArray();
                        rotorNotches = string.Empty.ToCharArray();
                        this.CanTurn = false;
                        break;
                    }

                case EnigmaRotorNumber.One:
                    {
                        rotorWiring = @"EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray();
                        rotorNotches = @"Q".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Two:
                    {
                        rotorWiring = @"AJDKSIRUXBLHWTMCQGZNPYFVOE".ToCharArray();
                        rotorNotches = @"E".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Three:
                    {
                        rotorWiring = @"BDFHJLCPRTXVZNYEIWGAKMUSQO".ToCharArray();
                        rotorNotches = @"V".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Four:
                    {
                        rotorWiring = @"ESOVPZJAYQUIRHXLNFTGKDCMWB".ToCharArray();
                        rotorNotches = @"J".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Five:
                    {
                        rotorWiring = @"VZBRGITYUPSDNHLXAWMJQOFECK".ToCharArray();
                        rotorNotches = @"Z".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Six:
                    {
                        rotorWiring = @"JPGVOUMFYQBENHZRDKASXLICTW".ToCharArray();
                        rotorNotches = @"MZ".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Seven:
                    {
                        rotorWiring = @"NZJHGRCXMYSWBOUFAIVLPEKQDT".ToCharArray();
                        rotorNotches = @"MZ".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Eight:
                    {
                        rotorWiring = @"FKQHTLXOCBJSPDZRAMEWNIUYGV".ToCharArray();
                        rotorNotches = @"MZ".ToCharArray();
                        this.CanTurn = true;
                        break;
                    }

                case EnigmaRotorNumber.Beta:
                    {
                        rotorWiring = @"LEYJVCNIXWPBQMDRTAKZGFUHOS".ToCharArray();
                        rotorNotches = string.Empty.ToCharArray();
                        this.CanTurn = false;
                        break;
                    }

                case EnigmaRotorNumber.Gamma:
                    {
                        rotorWiring = @"FSOKANUERHMBTIYCWLQPZXVGJD".ToCharArray();
                        rotorNotches = string.Empty.ToCharArray();
                        this.CanTurn = false;
                        break;
                    }

                default:
                    {
                        throw new CryptographicException("Unknown Enigma Rotor Number.");
                    }
            }

            Dictionary<char, char> rotorPairs = new Dictionary<char, char>();

            if (rotorWiring.Length == this.Letters.Count)
            {
                for (int i = 0; i < this.Letters.Count; i++)
                {
                    rotorPairs.Add(this.Letters[i], rotorWiring[i]);
                }
            }

            byte[] wiringKey = MonoAlphabeticSettings.BuildKey(new Collection<char>(this.Letters), rotorPairs, IsRotorSymmetric);
            byte[] wiringIV = MonoAlphabeticSettings.BuildIV();

            Contract.Assert(wiringKey != null);
            Contract.Assert(wiringIV != null);

            this.wiringSettings = new MonoAlphabeticSettings(wiringKey, wiringIV);

            byte[] rgbKey = this.wiringSettings.Key;
            byte[] rgbIV = this.wiringSettings.IV;

            Contract.Assert(rgbKey != null);
            Contract.Assert(rgbIV != null);

            CipherTransformMode transformMode = CipherTransformMode.Encrypt;

            Contract.Assume(Enum.IsDefined(typeof(CipherTransformMode), transformMode));

            this.wiring = new MonoAlphabeticTransform(rgbKey, rgbIV, transformMode);

            // Set the notches
            this.notches = new List<int>(rotorNotches.Length);
            foreach (char notch in rotorNotches)
            {
                this.notches.Add(this.Letters.IndexOf(notch));
            }
        }

        private void OnRotorAdvanced(bool isNotchHit, bool isDoubleStep)
        {
            if (this.RotorAdvanced != null)
            {
                this.RotorAdvanced(this, new EnigmaRotorAdvanceEventArgs(this.RotorNumber, isNotchHit, isDoubleStep));
            }
        }

        //private void OnRotorReversed()
        //{
        //    if (this.RotorReversed != null)
        //    {
        //        this.RotorReversed(this, new EnigmaRotorAdvanceEventArgs(this.RotorNumber, false));
        //    }
        //}

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            //Contract.Invariant(this.Letters != null);
            //Contract.Invariant(rotorLetters != null);
            //Contract.Invariant(wiring != null);
            //Contract.Invariant(wiringSettings != null);
            //Contract.Invariant(this.notches != null);
        }


        #endregion
    }
}
