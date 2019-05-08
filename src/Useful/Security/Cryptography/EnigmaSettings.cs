//-----------------------------------------------------------------------
// <copyright file="EnigmaSettings.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>The Enigma algorithm settings.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Linq;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The Enigma algorithm settings.
    /// </summary>
    public class EnigmaSettings : ISymmetricCipherSettings, INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// The seperator between key fields.
        /// </summary>
        private const char KeySeperator = '|';

        /// <summary>
        /// The seperator between values in a key field.
        /// </summary>
        internal const char KeyDelimiter = ' ';

        /// <summary>
        /// the number of fields in the key.
        /// </summary>
        private const int KeyParts = 5;

        /// <summary>
        /// The seperator between values in an IV field.
        /// </summary>
        private const char IVSeperator = ' ';

        private EnigmaModel model;

        /// <summary>
        /// Is the plugboard symmetric?
        /// </summary>
        private const bool IsPlugboardSymmetric = true;
        
        ///// <summary>
        ///// Rotors, sorted by position.
        ///// </summary>
        //private SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSettings;

        ///// <summary>
        ///// The setting for each rotor.
        ///// </summary>
        //private SortedDictionary<EnigmaRotorPosition, char> rotorSetting;

        ///// <summary>
        ///// Available rotors, sorted by position.
        ///// </summary>
        //private List<EnigmaRotorNumber> availableRotors;
        #endregion

        #region ctor

        /// <summary>
        /// Initializes a new instance of the EnigmaSettings class.
        /// </summary>
        private EnigmaSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the EnigmaSettings class.
        /// </summary>
        /// <param name="byteKey">The encryption Key.</param>
        /// <param name="byteIV">The Initialization Vector.</param>
        public EnigmaSettings(byte[] byteKey, byte[] byteIV)
        {
            Contract.Requires(byteKey != null);
            Contract.Requires(byteIV != null);

            // Reset rotor settings
            // this.rotorSettings = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings>(new EnigmaRotorPositionSorter(SortOrder.Ascending));
            // this.Rotors = new EnigmaRotorSettings();
            //this.AllowedRotorPositions = new Collection<EnigmaRotorPosition>();
            //this.availableRotors = new List<EnigmaRotorNumber>();
            this.AllowedLetters = new Collection<char>();
            this.CipherName = "Enigma";
            this.Plugboard = new MonoAlphabeticSettings(MonoAlphabeticSettings.BuildKey(Letters.EnglishAlphabetUppercase, new Dictionary<char, char>(), true), MonoAlphabeticSettings.BuildIV());

            this.Key = byteKey;
            this.IV = byteIV;

            Contract.Assert(!this.CipherName.Contains("\0"));
        }
        #endregion

        #region Events


        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the type of Enigma machine
        /// </summary>
        public EnigmaModel Model 
        {
            get
            {
                return this.model;
            }

            private set
            {
                // Contract.Requires(this.Rotors != null);
                this.model = value;

                this.AllowedLetters = EnigmaSettings.GetKeyboardLetters(value);
                //this.AllowedRotorPositions = EnigmaSettings.GetAllowedRotorPositions(this.Model);
                //this.availableRotors = new List<EnigmaRotorNumber>(EnigmaSettings.GetAllowedRotors(this.Model));
                //this.rotorSettings.Clear();

                //// Fill in the allowed and available rotors
                //foreach (EnigmaRotorPosition rotorPosition in this.AllowedRotorPositions)
                //{
                //    // Set an empty rotor in each position
                //    this.rotorSettings.Add(rotorPosition, EnigmaRotorSettings.Empty);
                //}

                this.Rotors = EnigmaRotorSettings.Create(value);

                // Plugboard
                byte[] plugboardKey = MonoAlphabeticSettings.BuildKey(this.AllowedLetters, new Dictionary<char, char>(), IsPlugboardSymmetric);
                byte[] plugboardIV = MonoAlphabeticSettings.BuildIV();
                //if (this.Plugboard == null)
                //{
                //    this.Plugboard = new MonoAlphabeticSettings(plugboardKey, plugboardIV);
                //    this.Plugboard.SettingsChanged += new EventHandler<EventArgs>(this.Plugboard_SettingsChanged);
                //}
                //else
                //{
                    this.Plugboard.Key = plugboardKey;
                    this.Plugboard.IV = plugboardIV;
                //}

                // Reflector
                this.ReflectorNumber = EnigmaReflector.GetDefault(this.Model).ReflectorNumber;
            }
        }

        /// <summary>
        /// Gets the allowed letters.
        /// </summary>
        public Collection<char> AllowedLetters { get; private set; }

        /// <summary>
        /// Gets the number of plugboard pairs that have been swapped.
        /// </summary>
        public int PlugboardSubstitutionCount
        {
            get
            {
                return this.Plugboard.SubstitutionCount;
            }
        }

        ///// <summary>
        ///// Gets the allowed rotor positions.
        ///// </summary>
        //public Collection<EnigmaRotorPosition> AllowedRotorPositions { get; private set; }

        ///// <summary>
        ///// Gets the rotor count.
        ///// </summary>
        //public int RotorPositionCount
        //{
        //    get
        //    {
        //        return this.AllowedRotorPositions.Count;
        //    }
        //}

        /// <summary>
        /// Gets the reflector being used.
        /// </summary>
        public EnigmaReflectorNumber ReflectorNumber { get; private set; }

        /// <summary>
        /// Gets the name of this cipher
        /// </summary>
        public string CipherName { get; private set; }

        public EnigmaRotorSettings Rotors { get; private set; }

        /// <summary>
        /// Gets the plugboard settings.
        /// </summary>
        internal MonoAlphabeticSettings Plugboard { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the allowed keyboard letters.
        /// </summary>
        /// <param name="model">The Enigma model to get the keyboard letters for.</param>
        /// <returns>The allowed keyboard letters for the specified model.</returns>
        public static Collection<char> GetKeyboardLetters(EnigmaModel model)
        {
            Contract.Requires(Enum.IsDefined(typeof(EnigmaModel), model));
            Contract.Ensures(Contract.Result<Collection<char>>() != null);

            switch (model)
            {
                case EnigmaModel.Military:
                default:
                    {
                        return new Collection<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
                    }
            }
        }

        ///// <summary>
        ///// Clears out all the Plugboard settings and replaces them with new ones.
        ///// </summary>
        ///// <param name="pairs">The new plugboard pairs to replace the old ones with.</param>
        //public void SetPlugboardNew(Collection<SubstitutionPair> pairs)
        //{
        //    Contract.Requires(pairs != null);

        //    SubstitutionPair.CheckPairs(EnigmaSettings.GetKeyboardLetters(this.Model), pairs, IsPlugboardSymmetric);

        //    // TODO: Check new pairs aren't the same as existing pairs
        //    this.Plugboard.SetSubstitutions(pairs);

        //    // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //    // It's all good, raise an event
        //    this.NotifyPropertyChanged();
        //}

        ///// <summary>
        ///// Sets a new Plugboard setting.
        ///// </summary>
        ///// <param name="pair">The new plugboard setting.</param>
        //public void SetPlugboardPair(SubstitutionPair pair)
        //{
        //    SubstitutionPair.CheckPairs(EnigmaSettings.GetKeyboardLetters(this.Model), new Collection<SubstitutionPair>() { pair }, false);

        //    this.Plugboard.SetSubstitution(pair);

        //    // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //    // It's all good, raise an event
        //    this.NotifyPropertyChanged();
        //}

        ///// <summary>
        ///// The rotors that can be used in this cipher.
        ///// </summary>
        ///// <param name="rotorPosition">The position to get the available rotors for.</param>
        ///// <returns>A collection of available rotors.</returns>
        //public IEnumerable<EnigmaRotorNumber> AvailableRotors()
        //{
        //    return this.availableRotors;

        //    // this.availableRotors.Sort(new EnigmaRotorNumberSorter(SortOrder.Ascending));
        //    // return new Collection<EnigmaRotorNumber>(this.availableRotors[rotorPosition].ToArray());
        //}

        ///// <summary>
        ///// Set the rotor order for this machine.
        ///// </summary>
        ///// <param name="rotorPosition">The position to place this rotor in.</param>
        ///// <param name="rotorNumber">The rotor to put in this position.</param>
        //public void SetRotorOrder(EnigmaRotorPosition rotorPosition, EnigmaRotorSettings rotor)
        //{
        //    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        //    {
        //        throw new ArgumentException("This position is not available.");
        //    }

        //    // Is the rotor being set to the existing one?
        //    if (this.rotorSettings[rotorPosition].RotorNumber == rotor.RotorNumber)
        //    {
        //        return;
        //    }

        //    if (!this.availableRotors.Contains(rotor.RotorNumber))
        //    {
        //        throw new ArgumentException("This rotor in this position is not available.");
        //    }

        //    this.SetRotorOrder_Private(rotorPosition, rotor);

        //    // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //    // It's all good, raise an event
        //    this.OnSettingsChanged();
        //}

        ///// <summary>
        ///// Set the rotor order for this machine.
        ///// </summary>
        ///// <param name="rotorPosition">The position to place this rotor in.</param>
        ///// <returns>The rotor to put in the position.</returns>
        //public EnigmaRotorNumber GetRotorOrder(EnigmaRotorPosition rotorPosition)
        //{
        //    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        //    {
        //        throw new ArgumentException("Invalid rotor position.");
        //    }

        //    return this.rotorSettings[rotorPosition].RotorNumber;
        //}

        ///// <summary>
        ///// Gets the rotor's ring setting.
        ///// </summary>
        ///// <param name="rotorPosition">The rotor position for which to get the ring setting.</param>
        ///// <returns>The ring setting for the specified position.</returns>
        //public char GetRingSetting(EnigmaRotorPosition rotorPosition)
        //{
        //    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        //    {
        //        throw new ArgumentException("This position is not allowed.");
        //    }

        //    if (this.rotorSettings[rotorPosition].IsEmpty)
        //    {
        //        throw new ArgumentException("No rotor currently in this position.");
        //    }

        //    if (!this.rotorSettings.ContainsKey(rotorPosition))
        //    {
        //        throw new ArgumentException("No rotor currently in this position.");
        //    }

        //    return this.rotorSettings[rotorPosition].RingPosition;
        //}

        ///// <summary>
        ///// Gets the rotor settings.
        ///// </summary>
        ///// <param name="rotorPosition">The rotor position for which to get the setting.</param>
        ///// <returns>The setting for the specified position.</returns>
        //public char GetRotorSetting(EnigmaRotorPosition rotorPosition)
        //{
        //    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        //    {
        //        throw new ArgumentException("This position is not allowed.");
        //    }

        //    if (this.Rotors[rotorPosition].IsNone)
        //    {
        //        throw new ArgumentException("No rotor currently in this position.");
        //    }

        //    if (!this.Rotors.ContainsKey(rotorPosition))
        //    {
        //        throw new ArgumentException("No rotor currently in this position.");
        //    }

        //    return this.rotorSettings[rotorPosition].CurrentSetting;
        //}

        ///// <summary>
        ///// Sets the rotor settings.
        ///// </summary>
        ///// <param name="rotorPosition">The rotor position to set.</param>
        ///// <param name="letter">The letter to set this position to.</param>
        //public void SetRotorSetting(EnigmaRotorPosition rotorPosition, char letter)
        //{
        //    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        //    {
        //        throw new ArgumentException("This position is not allowed.");
        //    }

        //    if (this.rotorSettings[rotorPosition].RotorNumber == EnigmaRotorNumber.None)
        //    {
        //        throw new ArgumentException("This setting is not allowed.");
        //    }

        //    if (!EnigmaRotor.GetAllowedLetters(this.rotorSettings[rotorPosition].RotorNumber).Contains(letter))
        //    {
        //        throw new ArgumentException("This setting is not allowed.");
        //    }

        //    this.SetRotorSetting_Private(rotorPosition, letter);

        //    // this.iv = EnigmaSettings.BuildIV(this.rotorSetting);
        //    this.OnSettingsChanged();
        //}

        ///// <summary>
        ///// Set the encryption Key.
        ///// </summary>
        ///// <param name="keyValue">The key for this cipher.</param>
        //public void SetKey(byte[] keyValue)
        //{
        //    //if (EnigmaSettings.BuildKey(this.Model, this.ReflectorNumber, this.Rotors, this.Plugboard) == keyValue)
        //    //{
        //    //    return;
        //    //}

        //    EnigmaSettings enigmaKey = ParseEnigmaKey(keyValue);

        //    // Model
        //    this.Model = enigmaKey.Model;
        //    // this.SetEnigmaModel();

        //    // Rotor Order
        //    this.Rotors = enigmaKey.Rotors;

        //    //foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> rotor in enigmaKey.RotorSettings)
        //    //{
        //    //    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorPosition), rotor.Key));
        //    //    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorNumber), rotor.Value.RotorNumber));
        //    //    this.Rotors[rotor.Key] = rotor.Value;
        //    //}

        //    // Plugboard
        //    // this.Plugboard.SetSubstitutions(enigmaKey.PlugboardPairs);
        //    this.Plugboard = enigmaKey.Plugboard;

        //    // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //    // No need to build IV?
        //    this.NotifyPropertyChanged();
        //}

        ///// <summary>
        ///// Get the encryption Key.
        ///// </summary>
        ///// <returns>The encryption key.</returns>
        //public byte[] GetKey()
        //{
        //    Contract.Ensures(Contract.Result<byte[]>() != null);

        //    byte[] key = EnigmaSettings.BuildKey(this.Model, this.ReflectorNumber, this.Rotors, this.Plugboard);

        //    Contract.Assert(key != null);

        //    return key;
        //}

        /// <summary>
        /// Get the encryption Key.
        /// </summary>
        public byte[] Key
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);

                byte[] key = EnigmaSettings.BuildKey(this.Model, this.ReflectorNumber, this.Rotors, this.Plugboard);

                //Contract.Assert(key != null);

                return key;
            }
            set
            {
                //if (EnigmaSettings.BuildKey(this.Model, this.ReflectorNumber, this.Rotors, this.Plugboard) == keyValue)
                //{
                //    return;
                //}

                EnigmaSettings settings = ParseKey(value);

                // Model
                this.Model = settings.Model;
                // this.SetEnigmaModel();

                // Rotor Order
                this.Rotors = settings.Rotors;

                //foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> rotor in enigmaKey.RotorSettings)
                //{
                //    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorPosition), rotor.Key));
                //    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorNumber), rotor.Value.RotorNumber));
                //    this.Rotors[rotor.Key] = rotor.Value;
                //}

                // Plugboard
                // this.Plugboard.SetSubstitutions(enigmaKey.PlugboardPairs);
                this.Plugboard = settings.Plugboard;

                // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

                // No need to build IV?
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Set the Initialization Vector.
        /// </summary>
        /// <param name="newValue">The new Initialization Vector.</param>
        public byte[] IV
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);

                byte[] iv = EnigmaSettings.BuildIV(this.Rotors);

                Contract.Assert(iv != null);

                return iv;
            }
            set
            {
                // Example:
                // G M Y
                this.CheckNullArgument(() => value);

                if (value.Length <= 0)
                {
                    throw new CryptographicException("No IV specified.");
                }

                if (EnigmaSettings.BuildIV(this.Rotors) == value)
                {
                    return;
                }

                char[] newChars = Encoding.Unicode.GetChars(value);

                string newString = new string(newChars);

                string[] parts = newString.Split(new char[] { IVSeperator }, StringSplitOptions.None);

                if (parts.Length > this.Rotors.Count)
                {
                    throw new ArgumentException("Too many IV parts specified.");
                }

                if (parts.Length < this.Rotors.Count)
                {
                    throw new ArgumentException("Too few IV parts specified.");
                }

                parts = parts.Reverse().ToArray();

                EnigmaRotorPosition rotorPosition;
                EnigmaRotorNumber rotorNumber;

                // Check that the rotor in the relevant position contains the specified letter
                for (int i = 0; i < parts.Length; i++)
                {
                    rotorPosition = (EnigmaRotorPosition)Enum.Parse(typeof(EnigmaRotorPosition), i.ToString(Culture.CurrentCulture));
                    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorPosition), rotorPosition));
                    rotorNumber = this.Rotors[rotorPosition].RotorNumber;
                    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorNumber), rotorNumber));

                    if (!EnigmaRotor.GetAllowedLetters(rotorNumber).Contains(parts[i][0]))
                    {
                        throw new ArgumentException("This setting is not allowed.");
                    }
                }

                for (int i = 0; i < parts.Length; i++)
                {
                    rotorPosition = (EnigmaRotorPosition)Enum.Parse(typeof(EnigmaRotorPosition), i.ToString(Culture.CurrentCulture));
                    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorPosition), rotorPosition));

                    this.Rotors[rotorPosition].CurrentSetting = parts[i][0];

                    // this.SetRotorSetting_Private(rotorPosition, parts[i][0]);
                }

                // this.iv = EnigmaSettings.BuildIV(this.rotorSetting);
                this.NotifyPropertyChanged();
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Enum.IsDefined(typeof(EnigmaModel), Model));
            //Contract.Invariant(AllowedLetters != null);
            //Contract.Invariant(PlugboardSubstitutionCount >= 0);
            //Contract.Invariant(this.AllowedRotorPositions != null);
            //Contract.Invariant(this.AllowedRotorPositions.Count >= 0);
            //Contract.Invariant(RotorPositionCount >= 0);
            //Contract.Invariant(Enum.IsDefined(typeof(EnigmaReflectorNumber), ReflectorNumber));
            //Contract.Invariant(CipherName != null);
            //Contract.Invariant(!CipherName.Contains("\0"));
            //Contract.Invariant(Plugboard != null);
            //Contract.Invariant(Enum.IsDefined(typeof(EnigmaModel), this.Model));
            //Contract.Invariant(this.Rotors != null);
            //Contract.Invariant(this.availableRotors != null); 
        }

        public static EnigmaSettings GetDefault()
        {
            byte[] key = EnigmaSettings.GetDefaultKey();
            byte[] iv = EnigmaSettings.GetDefaultIV(key);
            EnigmaSettings settings = new EnigmaSettings(key, iv);
            return settings;
        }

        public static EnigmaSettings GetRandom()
        {
            byte[] key = EnigmaSettings.GetRandomKey();
            byte[] iv = EnigmaSettings.GetRandomIV(key);
            EnigmaSettings settings = new EnigmaSettings(key, iv);
            return settings;
        }

        /// <summary>
        /// Returns the default initialization vector.
        /// </summary>
        /// <param name="key">The key to base the initialization vector on.</param>
        /// <returns>The default initialization vector.</returns>
        private static byte[] GetDefaultIV(byte[] key)
        {
            Contract.Requires(key != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            EnigmaSettings enigmaKey = EnigmaSettings.ParseKey(key);
            EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.GetDefault(enigmaKey.Model);
            byte[] iv = EnigmaSettings.BuildIV(rotorSettings);
            return iv;
        }

        /// <summary>
        /// Returns a randomly generated initialization vector.
        /// </summary>
        /// <param name="key">The key to base the initialization vector on.</param>
        /// <returns>A randomly generated initialization vector.</returns>
        private static byte[] GetRandomIV(byte[] key)
        {
            Contract.Requires(key != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            EnigmaSettings enigmaKey = EnigmaSettings.ParseKey(key);
            EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.GetRandom(enigmaKey.Model);
            byte[] iv = EnigmaSettings.BuildIV(rotorSettings);
            return iv;
        }

        /// <summary>
        /// Returns the default initialization key.
        /// </summary>
        /// <returns>The default initialization key.</returns>
        [Pure]
        private static byte[] GetDefaultKey()
        {
            // Model
            EnigmaModel model = EnigmaModel.Military;

            // Rotor Settings
            EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.GetDefault(model);

            // Plugboard
            MonoAlphabeticSettings plugboard = MonoAlphabeticSettings.GetDefault(EnigmaSettings.GetKeyboardLetters(model));

            byte[] key = EnigmaSettings.BuildKey(model, EnigmaReflectorNumber.B, rotorSettings, plugboard);
            return key;
        }

        /// <summary>
        /// Returns a randomly generated initialization key.
        /// </summary>
        /// <returns>A randomly generated initialization key.</returns>
        private static byte[] GetRandomKey()
        {
            // Model
            EnigmaModel model = EnigmaSettings.GetRandomModel();

            // Reflector
            EnigmaReflectorNumber reflector = EnigmaReflector.GetRandom(model).ReflectorNumber;

            // Rotor Settings
            EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.GetRandom(model);

            // Plugboard
            Collection<char> allowedLetters = EnigmaSettings.GetKeyboardLetters(model);
            MonoAlphabeticSettings plugboard = MonoAlphabeticSettings.GetRandom(allowedLetters, true);

            byte[] key = EnigmaSettings.BuildKey(model, reflector, rotorSettings, plugboard);
            return key;
        }

        //private static List<EnigmaReflectorNumber> GetAllowedReflectors(EnigmaModel model)
        //{
        //    switch (model)
        //    {
        //        case (EnigmaModel.Military):
        //        case (EnigmaModel.M3):
        //            {
        //                return new List<EnigmaReflectorNumber>(2) {
        //                    EnigmaReflectorNumber.B,
        //                    EnigmaReflectorNumber.C
        //                    };
        //            }

        //        case (EnigmaModel.M4):
        //            {
        //                return new List<EnigmaReflectorNumber>(2) {
        //                    EnigmaReflectorNumber.BThin,
        //                    EnigmaReflectorNumber.CThin
        //                    };
        //            }
        //        default:
        //            {
        //                throw new CryptographicException("Unknown Enigma model.");
        //            }
        //    }
        //}

        ///// <summary>
        ///// Gets all the allowed rotors for a given Enigma machine type.
        ///// </summary>
        ///// <param name="model">The specified Enigma machine type.</param>
        ///// <returns>All the allowed rotors for the machine type and position.</returns>
        //private static List<EnigmaRotorNumber> GetAllowedRotors(EnigmaModel model)
        //{
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaModel), model));

        //    switch (model)
        //    {
        //        case EnigmaModel.Military:
        //            {
        //                return new List<EnigmaRotorNumber>(6) { 
        //                    EnigmaRotorNumber.None, 
        //                    EnigmaRotorNumber.One, 
        //                    EnigmaRotorNumber.Two, 
        //                    EnigmaRotorNumber.Three, 
        //                    EnigmaRotorNumber.Four, 
        //                    EnigmaRotorNumber.Five
        //                };
        //            }

        //        case EnigmaModel.M3:
        //            {
        //                return new List<EnigmaRotorNumber>(9) { 
        //                    EnigmaRotorNumber.None, 
        //                    EnigmaRotorNumber.One, 
        //                    EnigmaRotorNumber.Two, 
        //                    EnigmaRotorNumber.Three, 
        //                    EnigmaRotorNumber.Four, 
        //                    EnigmaRotorNumber.Five,
        //                    EnigmaRotorNumber.Six,
        //                    EnigmaRotorNumber.Seven,
        //                    EnigmaRotorNumber.Eight
        //                };
        //            }

        //        case EnigmaModel.M4:
        //            {
        //                return new List<EnigmaRotorNumber>(11) { 
        //                    EnigmaRotorNumber.None, 
        //                    EnigmaRotorNumber.One, 
        //                    EnigmaRotorNumber.Two, 
        //                    EnigmaRotorNumber.Three, 
        //                    EnigmaRotorNumber.Four, 
        //                    EnigmaRotorNumber.Five,
        //                    EnigmaRotorNumber.Six,
        //                    EnigmaRotorNumber.Seven,
        //                    EnigmaRotorNumber.Eight,
        //                    EnigmaRotorNumber.Beta,
        //                    EnigmaRotorNumber.Gamma
        //                };
        //            }

        //        default:
        //            {
        //                throw new ArgumentException("Unknown Enigma model.");
        //            }
        //    }
        //}

        ///// <summary>
        ///// Gets the allowed rotors for a given rotor position in a given Enigma machine type.
        ///// </summary>
        ///// <param name="model">The specified Enigma machine type.</param>
        ///// <param name="rotorPosition">The specified rotor position.</param>
        ///// <returns>The allowed rotors for the machine type and position.</returns>
        //internal static List<EnigmaRotorNumber> GetAllowedRotors(EnigmaModel model, EnigmaRotorPosition rotorPosition)
        //{
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaModel), model));
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaRotorPosition), rotorPosition));

        //    Contract.Requires(EnigmaSettings.GetAllowedRotorPositions(model).Contains(rotorPosition));

        //    List<EnigmaRotorNumber> allowedRotors = new List<EnigmaRotorNumber>();
                        
        //    allowedRotors.Add(EnigmaRotorNumber.None);

        //    switch (model)
        //    {
        //        case EnigmaModel.Military:
        //            {
        //                allowedRotors.Add(EnigmaRotorNumber.One);
        //                allowedRotors.Add(EnigmaRotorNumber.Two);
        //                allowedRotors.Add(EnigmaRotorNumber.Three);
        //                allowedRotors.Add(EnigmaRotorNumber.Four);
        //                allowedRotors.Add(EnigmaRotorNumber.Five);

        //                return allowedRotors;
        //            }

        //        case EnigmaModel.M3:
        //            {
        //                allowedRotors.Add(EnigmaRotorNumber.One);
        //                allowedRotors.Add(EnigmaRotorNumber.Two);
        //                allowedRotors.Add(EnigmaRotorNumber.Three);
        //                allowedRotors.Add(EnigmaRotorNumber.Four);
        //                allowedRotors.Add(EnigmaRotorNumber.Five);
        //                allowedRotors.Add(EnigmaRotorNumber.Six);
        //                allowedRotors.Add(EnigmaRotorNumber.Seven);
        //                allowedRotors.Add(EnigmaRotorNumber.Eight);

        //                return allowedRotors;
        //            }

        //        case EnigmaModel.M4:
        //            {
        //                if (rotorPosition == EnigmaRotorPosition.Fastest
        //                    || rotorPosition == EnigmaRotorPosition.Second
        //                    || rotorPosition == EnigmaRotorPosition.Third)
        //                {
        //                    allowedRotors.Add(EnigmaRotorNumber.One);
        //                    allowedRotors.Add(EnigmaRotorNumber.Two);
        //                    allowedRotors.Add(EnigmaRotorNumber.Three);
        //                    allowedRotors.Add(EnigmaRotorNumber.Four);
        //                    allowedRotors.Add(EnigmaRotorNumber.Five);
        //                    allowedRotors.Add(EnigmaRotorNumber.Six);
        //                    allowedRotors.Add(EnigmaRotorNumber.Seven);
        //                    allowedRotors.Add(EnigmaRotorNumber.Eight);
        //                }
        //                else if (rotorPosition == EnigmaRotorPosition.Forth)
        //                {
        //                    allowedRotors.Add(EnigmaRotorNumber.Beta);
        //                    allowedRotors.Add(EnigmaRotorNumber.Gamma);
        //                }
        //                return allowedRotors;
        //            }

        //        default:
        //            {
        //                throw new ArgumentException("Unknown Enigma model.");
        //            }
        //    }
        //}

        ///// <summary>
        ///// Returns the available rotors for a given Enigma model and given position.
        ///// </summary>
        ///// <param name="model">The specified Enigma model.</param>
        ///// <param name="rotorsByPosition">The rotors currently in the positions.</param>
        ///// <param name="rotorPosition">Which position to get the availablity for.</param>
        ///// <returns>The available rotors for a given Enigma model and given position.</returns>
        //private static List<EnigmaRotorNumber> GetAvailableRotors(EnigmaModel model, SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSettings, EnigmaRotorPosition rotorPosition)
        //{
        //    Contract.Requires(rotorSettings != null);
        //    Contract.Assert(EnigmaSettings.GetAllowedRotorPositions(model).Contains(rotorPosition));

        //    List<EnigmaRotorNumber> allowedRotors = GetAllowedRotors(model, rotorPosition);

        //    foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorSettings> rotorSetting in rotorSettings)
        //    {
        //        Contract.Assert(EnigmaSettings.GetAllowedRotorPositions(model).Contains(rotorSetting.Key));
        //        //Contract.Assert(EnigmaSettings.GetAllowedRotors(model, rotorSettings[rotorSettings].RingPosition).Contains(rotorByPosition.Value));

        //        if (rotorSetting.Value.RotorNumber != EnigmaRotorNumber.None)
        //        {
        //            if (allowedRotors.Contains(rotorSetting.Value.RotorNumber))
        //            {
        //                allowedRotors.Remove(rotorSetting.Value.RotorNumber);
        //            }
        //        }
        //    }

        //    return allowedRotors;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //internal static Collection<EnigmaRotorPosition> GetAllowedRotorPositions(EnigmaModel model)
        //{
        //    Collection<EnigmaRotorPosition> allowedRotorPositions = new Collection<EnigmaRotorPosition>();

        //    switch (model)
        //    {
        //        case EnigmaModel.Military:
        //            {
        //                allowedRotorPositions.Add(EnigmaRotorPosition.Fastest);
        //                allowedRotorPositions.Add(EnigmaRotorPosition.Second);
        //                allowedRotorPositions.Add(EnigmaRotorPosition.Third);
        //                break;
        //            }
        //        case EnigmaModel.M3:
        //        case EnigmaModel.M4:
        //            {
        //                allowedRotorPositions.Add(EnigmaRotorPosition.Fastest);
        //                allowedRotorPositions.Add(EnigmaRotorPosition.Second);
        //                allowedRotorPositions.Add(EnigmaRotorPosition.Third);
        //                allowedRotorPositions.Add(EnigmaRotorPosition.Forth);
        //                break;
        //            }

        //        default:
        //            {
        //                throw new ArgumentException("Unknown Enigma model.");
        //            }
        //    }

        //    return allowedRotorPositions;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rotorOrder"></param>
        /// <param name="plugboard"></param>
        /// <returns></returns>
        private static byte[] BuildKey(
            EnigmaModel model,
            EnigmaReflectorNumber reflector,
            EnigmaRotorSettings rotors,
            MonoAlphabeticSettings plugboard)
        {
            // Example:
            // "model|reflector|rotors|ring|plugboard"
            // "Military|B|III II I|C B A|DN GR IS KC QX TM PV HY FW BJ"
            Contract.Requires(Enum.IsDefined(typeof(EnigmaModel), model));
            Contract.Requires(rotors != null);
            Contract.Requires(plugboard != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            // Contract.Assume(SubstitutionPair.CheckPairs(EnigmaSettings.GetKeyboardLetters(model), plugboard.Substitutions, IsPlugboardSymmetric));

            StringBuilder key = new StringBuilder();

            // Model
            key.Append(model.ToString());
            key.Append(KeySeperator);

            // Reflector
            key.Append(reflector.ToString());
            key.Append(KeySeperator);

            // Rotor order
            key.Append(rotors.GetOrderKey());
            key.Append(KeySeperator);

            // Ring setting
            key.Append(rotors.GetRingKey());
            key.Append(KeySeperator);

            // Plugboard
            key.Append(plugboard.GetSettingKey());

            // Contract.Assume(Encoding.Unicode.GetBytes(key.ToString()) != null);

            return Encoding.Unicode.GetBytes(key.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorSetting"></param>
        /// <returns></returns>
        [Pure]
        private static byte[] BuildIV(EnigmaRotorSettings rotorSettings)
        {
            // Example:
            // G M Y
            Contract.Requires(rotorSettings != null);
            //Contract.Requires(rotorSettings.Values != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            byte[] result = Encoding.Unicode.GetBytes(rotorSettings.GetSettingKey());

            Contract.Assert(result != null);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorPosition"></param>
        /// <param name="currentSetting"></param>
        internal void AdvanceRotor(EnigmaRotorPosition rotorPosition, char currentSetting)
        {
            //Contract.Assert(this.AllowedRotorPositions.Contains(rotorPosition));
            Contract.Assert(this.AllowedLetters.Contains(currentSetting));

            this.Rotors[rotorPosition].CurrentSetting = currentSetting;

            // this.SetRotorSetting(rotorPosition, currentSetting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static EnigmaModel GetDefaultModel()
        {
            return EnigmaModel.Military;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static EnigmaModel GetRandomModel()
        {
            Random rnd = new Random();

            int i = rnd.Next(0, Enum.GetValues(typeof(EnigmaModel)).Length - 1);
            EnigmaModel model = (EnigmaModel)Enum.Parse(typeof(EnigmaModel), i.ToString(CultureInfo.CurrentCulture), true);
            return model;
        }

        //private static EnigmaReflectorNumber GetDefaultReflector(EnigmaModel model)
        //{
        //    switch (model)
        //    {
        //        case EnigmaModel.Military:
        //            return EnigmaReflectorNumber.B;
        //        case EnigmaModel.M3:
        //        case EnigmaModel.M4:
        //            return EnigmaReflectorNumber.BThin;
        //        default:
        //            throw new CryptographicException("Unknown Enigma model.");
        //    }
        //}

        //private static EnigmaReflectorNumber GetRandomReflector(EnigmaModel model)
        //{
        //    Random rnd = new Random();

        //    List<EnigmaReflectorNumber> reflectors = GetAllowedReflectors(model);

        //    int nextRandomNumber = rnd.Next(0, reflectors.Count);

        //    return reflectors[nextRandomNumber];
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //private static SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> GetRandomRotorSettings(EnigmaModel model)
        //{
        //    Random rnd = new Random();
        //    int nextRandomNumber;
        //    SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSettings = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings>();

        //    Collection<EnigmaRotorPosition> allowedRotorPositions = EnigmaSettings.GetAllowedRotorPositions(model);

        //    List<EnigmaRotorNumber> availableRotorNumbers;
        //    foreach (EnigmaRotorPosition rotorPosition in allowedRotorPositions)
        //    {
        //        availableRotorNumbers = EnigmaSettings.GetAvailableRotors(model, rotorSettings, rotorPosition);
        //        if (availableRotorNumbers.Contains(EnigmaRotorNumber.None))
        //        {
        //            availableRotorNumbers.Remove(EnigmaRotorNumber.None);
        //        }

        //        nextRandomNumber = rnd.Next(0, availableRotorNumbers.Count);

        //        rotorSettings[rotorPosition] = GetRandomRotorSettings(availableRotorNumbers[nextRandomNumber]);
        //    }

        //    return rotorSettings;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static EnigmaSettings ParseKey(byte[] key)
        {
            // Example:
            // model|reflector|rotors|ring|plugboard
            Contract.Requires(key != null);
            Contract.Ensures(Contract.Result<EnigmaSettings>() != null);

            EnigmaSettings settings = new EnigmaSettings();

            char[] tempKey = Encoding.Unicode.GetChars(key);
            string keyString = new string(tempKey);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.");
            }

            // Model
            if (string.IsNullOrEmpty(parts[0]))
            {
                throw new ArgumentException("Model has not been specified.");
            }

            Contract.Assert(parts[0] != null);

            settings.model = (EnigmaModel)Enum.Parse(typeof(EnigmaModel), parts[0]);
            Contract.Assume(Enum.IsDefined(typeof(EnigmaModel), settings.model));

            // Reflector
            settings.ReflectorNumber = (EnigmaReflectorNumber)Enum.Parse(typeof(EnigmaReflectorNumber), parts[1]);
            if (!EnigmaReflector.GetAllowed(settings.model).Contains(settings.ReflectorNumber))
            {
                throw new ArgumentException("This reflector is not available.");
            }

            // Rotor Order
            string[] rotors = parts[2].Split(new char[] { KeyDelimiter });
            if (rotors.Length <= 0)
            {
                throw new ArgumentException("No rotors specified.");
            }

            int rotorPositionsCount = EnigmaRotorSettings.GetAllowedRotorPositions(settings.model).Count;

            if (rotors.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rotors specified.");
            }

            if (rotors.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rotors specified.");
            }

            if (rotors[0].Length == 0)
            {
                throw new ArgumentException("No rotors specified.");
            }

            rotors = rotors.Reverse().ToArray();

            // Ring
            string[] rings = parts[3].Split(new char[] { KeyDelimiter });

            if (rings.Length <= 0)
            {
                throw new ArgumentException("No rings specified.");
            }

            if (rings.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rings specified.");
            }

            if (rings.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rings specified.");
            }

            if (rings[0].Length == 0)
            {
                throw new ArgumentException("No rings specified.");
            }
            
            rings = rings.Reverse().ToArray();

            EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.Create(settings.model);

            for (int i = 0; i < rotors.Length; i++)
            {
                if (string.IsNullOrEmpty(rotors[i]) || rotors[i].Contains("\0"))
                {
                    throw new ArgumentException("Null or empty rotor specified.");
                }
                Contract.Assume(rotors[i].Length > 0);

                EnigmaRotorPosition rotorPosition = (EnigmaRotorPosition)Enum.Parse(typeof(EnigmaRotorPosition), i.ToString(Culture.CurrentCulture));
                EnigmaRotorNumber rotorNumber = EnigmaUINameConverter.Convert(rotors[i]);

                if (!rotorSettings.GetAvailableRotors(rotorPosition).Contains(rotorNumber))
                {
                    throw new ArgumentException("This rotor in this position is not available.");
                }

                EnigmaRotor enigmaRotor = EnigmaRotor.Create(rotorNumber);
                if (!enigmaRotor.Letters.Contains(rings[i][0]))
                {
                    throw new ArgumentException("This ring position is invalid.");
                }

                enigmaRotor.RingPosition = rings[i][0];
                // enigmaRotor.CurrentSetting = 'A';
                rotorSettings[rotorPosition] = enigmaRotor;
            }

            settings.Rotors = rotorSettings;

            // Plugboard
            string plugs = parts[4];

            //if (string.IsNullOrEmpty(plugs) || plugs.Contains("\0"))
            //{
            //    throw new ArgumentException("Null or empty plugs specified.");
            //}

            Dictionary<char, char> pairs = MonoAlphabeticSettings.GetPairs(EnigmaSettings.GetKeyboardLetters(settings.model), plugs, KeyDelimiter, true);

            byte[] plugboardKey = MonoAlphabeticSettings.BuildKey(EnigmaSettings.GetKeyboardLetters(settings.model), pairs, true);
            byte[] plugboardIv = MonoAlphabeticSettings.BuildIV();

            settings.Plugboard = new MonoAlphabeticSettings(plugboardKey, plugboardIv);

            return settings;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="rotorOrder"></param>
        ///// <returns></returns>
        //private static EnigmaRotorSettings GetDefaultRotorSettings(EnigmaRotorSettings rotorOrder)
        //{
        //    Contract.Requires(rotorOrder != null);

        //    SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSetting = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings>();

        //    foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorSettings> rotor in rotorOrder)
        //    {
        //        Collection<char> letters = EnigmaRotor.GetAllowedLetters(rotor.Value.RotorNumber);
        //        EnigmaRotorSettings setting = new EnigmaRotorSettings(rotor.Value.RotorNumber, letters[0], letters[0]);
        //        rotorSetting.Add(rotor.Key, setting);
        //    }

        //    return rotorSetting;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="rotorOrder"></param>
        ///// <returns></returns>
        //private static EnigmaRotorSettings GetRandomRotorSettings(EnigmaRotorNumber rotorNumber)
        //{
        //    Random rnd = new Random();
        //    Collection<char> letters = EnigmaRotor.GetAllowedLetters(rotorNumber);

        //    int index1 = rnd.Next(0, letters.Count);
        //    int index2 = rnd.Next(0, letters.Count);
        //    return new EnigmaRotorSettings(rotorNumber, letters[index1], letters[index2]);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="rotorPosition"></param>
        ///// <param name="letter"></param>
        //private void SetRotorSetting_Private(EnigmaRotorPosition rotorPosition, char letter)
        //{
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaRotorPosition), rotorPosition));
        //    Contract.Requires(this.rotorSettings != null);
        //    Contract.Requires(this.AllowedRotorPositions.Contains(rotorPosition));

        //    // Contract.Assert(this.rotorsByPosition[rotorPosition] != EnigmaRotorNumber.None);
        //    // Contract.Assert(EnigmaRotor.GetAllowedLetters(this.rotorsByPosition[rotorPosition]).Contains(letter));

        //    if (this.rotorSettings.ContainsKey(rotorPosition))
        //    {
        //        if (this.rotorSettings[rotorPosition].CurrentSetting == letter)
        //        {
        //            // Nothing to set, so return
        //            return;
        //        }

        //        this.rotorSettings[rotorPosition].CurrentSetting = letter;
        //    }
        //    else
        //    {
        //        throw new Exception();
        //        // this.rotorSetting.Add(rotorPosition, letter);
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //private void SetEnigmaModel()
        //{
        //    Contract.Requires(this.Rotors != null);

        //    this.AllowedLetters = EnigmaSettings.GetKeyboardLetters(this.Model);
        //    //this.AllowedRotorPositions = EnigmaSettings.GetAllowedRotorPositions(this.Model);
        //    //this.availableRotors = new List<EnigmaRotorNumber>(EnigmaSettings.GetAllowedRotors(this.Model));
        //    //this.rotorSettings.Clear();

        //    //// Fill in the allowed and available rotors
        //    //foreach (EnigmaRotorPosition rotorPosition in this.AllowedRotorPositions)
        //    //{
        //    //    // Set an empty rotor in each position
        //    //    this.rotorSettings.Add(rotorPosition, EnigmaRotorSettings.Empty);
        //    //}

        //    // Plugboard
        //    byte[] plugboardKey = MonoAlphabeticSettings.BuildKey(this.AllowedLetters, new Collection<SubstitutionPair>(), IsPlugboardSymmetric);
        //    byte[] plugboardIV = MonoAlphabeticSettings.BuildIV();
        //    if (this.Plugboard == null)
        //    {
        //        this.Plugboard = new MonoAlphabeticSettings(plugboardKey, plugboardIV);
        //        this.Plugboard.SettingsChanged += new EventHandler<EventArgs>(this.Plugboard_SettingsChanged);
        //    }
        //    else
        //    {
        //        this.Plugboard.SetKey(plugboardKey);
        //        this.Plugboard.SetIV(plugboardIV);
        //    }

        //    // Reflector
        //    this.ReflectorNumber = EnigmaSettings.GetDefaultReflector(this.Model);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void Plugboard_SettingsChanged(object sender, EventArgs e)
        //{
        //    this.NotifyPropertyChanged();
        //}

        ///// <summary>
        ///// Set the rotor order for this machine.
        ///// </summary>
        ///// <param name="rotorPosition">The position to place this rotor in.</param>
        ///// <param name="rotorNumber">The rotor to put in this position.</param>
        //private void SetRotorOrder_Private(EnigmaRotorPosition rotorPosition, EnigmaRotorSettings rotor)
        //{
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaRotorPosition), rotorPosition));
        //    Contract.Requires(rotor != null);
        //    // Contract.Requires(this.AllowedRotorPositions.Contains(rotorPosition));
        //    // Contract.Requires(this.availableRotorsByPosition[rotorPosition].Contains(rotorNumber));

        //    // Contract.Assert(Enum.IsDefined(typeof(EnigmaRotorNumber), rotorNumber));

        //    if (rotor.RotorNumber == this.rotorSettings[rotorPosition].RotorNumber)
        //    {
        //        return;
        //    }

        //    EnigmaRotorNumber currentRotor = this.rotorSettings[rotorPosition].RotorNumber;

        //    if (!this.availableRotors.Contains(currentRotor))
        //    {
        //        this.availableRotors.Add(currentRotor);
        //    }

        //    if (!rotor.IsEmpty)
        //    {
        //        this.availableRotors.Remove(rotor.RotorNumber);
        //    }

        //    this.availableRotors.Sort(new EnigmaRotorNumberSorter(SortOrder.Ascending));

        //    // Set the rotor order
        //    if (this.rotorSettings.ContainsKey(rotorPosition))
        //    {
        //        this.rotorSettings[rotorPosition] = rotor;
        //    }
        //    else
        //    {
        //        this.rotorSettings.Add(rotorPosition, rotor);
        //    }

        //    if (!this.rotorSettings.ContainsKey(rotorPosition))
        //    {
        //        Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorNumber), rotor.RotorNumber));
        //        Collection<char> allowedLetters = EnigmaRotor.GetAllowedLetters(rotor.RotorNumber);
        //        if (allowedLetters.Count > 0)
        //        {
        //            this.Rotors[rotorPosition].CurrentSetting = allowedLetters[0];

        //            // this.SetRotorSetting_Private(rotorPosition, allowedLetters[0]);
        //        }
        //    }
        //}

        internal static int GetIvLength(EnigmaModel model)
        {
            switch (model)
            {
                case EnigmaModel.Military:
                    return 5;
                case EnigmaModel.M3:
                case EnigmaModel.M4:
                    return 7;
                default:
                    throw new CryptographicException("Unknown Enigma model.");
            }
        }
        #endregion

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
