// <copyright file="EnigmaSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The Enigma algorithm settings.
    /// </summary>
    public class EnigmaSettings : CipherSettings
    {
        /// <summary>
        /// The seperator between values in a key field.
        /// </summary>
        internal const char KeyDelimiter = ' ';

        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /////// <summary>
        /////// Is the plugboard symmetric?.
        /////// </summary>
        ////private const bool IsPlugboardSymmetric = true;

        /////// <summary>
        /////// The seperator between values in an IV field.
        /////// </summary>
        ////private const char IVSeperator = ' ';

        /// <summary>
        /// the number of fields in the key.
        /// </summary>
        private const int KeyParts = 5;

        /// <summary>
        /// The seperator between key fields.
        /// </summary>
        private const char KeySeperator = '|';

        private const string _defaultKey = "Military|B|I II III|A A A|";

        private const string _defaultIv = "A A A";

        ///// <summary>
        ///// Rotors, sorted by position.
        ///// </summary>
        // private SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSettings;

        ///// <summary>
        ///// The setting for each rotor.
        ///// </summary>
        // private SortedDictionary<EnigmaRotorPosition, char> rotorSetting;

        ///// <summary>
        ///// Available rotors, sorted by position.
        ///// </summary>
        // private List<EnigmaRotorNumber> availableRotors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSettings"/> class.
        /// </summary>
        public EnigmaSettings()
            : this(Encoding.Unicode.GetBytes(_defaultKey), Encoding.Unicode.GetBytes(_defaultIv))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSettings"/> class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public EnigmaSettings(byte[] key, byte[] iv)
            : this(GetSettings(key, iv))
        {
            // Reset rotor settings
            // this.rotorSettings = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings>(new EnigmaRotorPositionSorter(SortOrder.Ascending));
            // this.Rotors = new EnigmaRotorSettings();
            // this.AllowedRotorPositions = new Collection<EnigmaRotorPosition>();
            // this.availableRotors = new List<EnigmaRotorNumber>();
            // this.Plugboard = new MonoAlphabeticSettings(CharacterSet.ToList(), new Dictionary<char, char>(), true);

            ////Key = byteKey;
            ////IV = byteIV;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSettings"/> class.
        /// </summary>
        /// <param name="reflector">The reflector.</param>
        /// <param name="rotorSettings">The rotor settings.</param>
        /// <param name="plugboard">The plugboard.</param>
        public EnigmaSettings(EnigmaReflectorNumber reflector, EnigmaRotorSettings rotorSettings, MonoAlphabeticSettings plugboard)
            : base()
        {
            ReflectorNumber = reflector;
            Rotors = rotorSettings;
            Plugboard = plugboard;
        }

        private EnigmaSettings((EnigmaReflectorNumber reflectorNumber, EnigmaRotorSettings rotorSettings, MonoAlphabeticSettings plugboard) settings)
            : this(settings.reflectorNumber, settings.rotorSettings, settings.plugboard)
        {
        }

        /// <inheritdoc/>
        public override IEnumerable<byte> IV
        {
            get
            {
                byte[] iv = EnigmaSettings.BuildIV(Rotors);

                return iv;
            }

            ////set
            ////{
            ////    // Example:
            ////    // G M Y
            ////    this.CheckNullArgument(() => value);

            ////    if (value.Length <= 0)
            ////    {
            ////        throw new CryptographicException("No IV specified.");
            ////    }

            ////    if (EnigmaSettings.BuildIV(Rotors) == value)
            ////    {
            ////        return;
            ////    }

            ////    char[] newChars = Encoding.Unicode.GetChars(value);

            ////    string newString = new string(newChars);

            ////    string[] parts = newString.Split(new char[] { IVSeperator }, StringSplitOptions.None);

            ////    if (parts.Length > Rotors.Count)
            ////    {
            ////        throw new ArgumentException("Too many IV parts specified.");
            ////    }

            ////    if (parts.Length < Rotors.Count)
            ////    {
            ////        throw new ArgumentException("Too few IV parts specified.");
            ////    }

            ////    parts = parts.Reverse().ToArray();

            ////    EnigmaRotorPosition rotorPosition;
            ////    EnigmaRotorNumber rotorNumber;

            ////    // Check that the rotor in the relevant position contains the specified letter
            ////    for (int i = 0; i < parts.Length; i++)
            ////    {
            ////        rotorPosition = (EnigmaRotorPosition)Enum.Parse(typeof(EnigmaRotorPosition), i.ToString(Culture.CurrentCulture));
            ////        rotorNumber = Rotors[rotorPosition].RotorNumber;

            ////        if (!AllowedLetters.Contains(parts[i][0]))
            ////        {
            ////            throw new ArgumentException("This setting is not allowed.");
            ////        }
            ////    }

            ////    for (int i = 0; i < parts.Length; i++)
            ////    {
            ////        rotorPosition = (EnigmaRotorPosition)Enum.Parse(typeof(EnigmaRotorPosition), i.ToString(Culture.CurrentCulture));

            ////        Rotors[rotorPosition].CurrentSetting = parts[i][0];

            ////        // this.SetRotorSetting_Private(rotorPosition, parts[i][0]);
            ////    }

            ////    // this.iv = EnigmaSettings.BuildIV(this.rotorSetting);
            ////    NotifyPropertyChanged();
            ////}
        }

        /// <inheritdoc/>
        public override IEnumerable<byte> Key
        {
            get
            {
                byte[] key = BuildKey(ReflectorNumber, Rotors, Plugboard);

                return key;
            }
        }

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
        // public Collection<EnigmaRotorPosition> AllowedRotorPositions { get; private set; }

        ///// <summary>
        ///// Gets the rotor count.
        ///// </summary>
        // public int RotorPositionCount
        // {
        //    get
        //    {
        //        return this.AllowedRotorPositions.Count;
        //    }
        // }

        /// <summary>
        /// Gets the reflector being used.
        /// </summary>
        public EnigmaReflectorNumber ReflectorNumber { get; private set; }

        /// <summary>
        /// Gets the rotors.
        /// </summary>
        public EnigmaRotorSettings Rotors { get; private set; }

        /// <summary>
        /// Gets or sets the plugboard settings.
        /// </summary>
        public MonoAlphabeticSettings Plugboard { get; set; }

        //// return key;
        //// }

        /////// <summary>
        /////// Clears out all the Plugboard settings and replaces them with new ones.
        /////// </summary>
        /////// <param name="pairs">The new plugboard pairs to replace the old ones with.</param>
        //// public void SetPlugboardNew(Collection<SubstitutionPair> pairs)
        //// {
        ////    Contract.Requires(pairs != null);

        //// SubstitutionPair.CheckPairs(EnigmaSettings.GetKeyboardLetters(this.Model), pairs, IsPlugboardSymmetric);

        //// // TODO: Check new pairs aren't the same as existing pairs
        ////    this.Plugboard.SetSubstitutions(pairs);

        //// // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //// // It's all good, raise an event
        ////    this.NotifyPropertyChanged();
        //// }

        /////// <summary>
        /////// Sets a new Plugboard setting.
        /////// </summary>
        /////// <param name="pair">The new plugboard setting.</param>
        //// public void SetPlugboardPair(SubstitutionPair pair)
        //// {
        ////    SubstitutionPair.CheckPairs(EnigmaSettings.GetKeyboardLetters(this.Model), new Collection<SubstitutionPair>() { pair }, false);

        //// this.Plugboard.SetSubstitution(pair);

        //// // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //// // It's all good, raise an event
        ////    this.NotifyPropertyChanged();
        //// }

        /////// <summary>
        /////// The rotors that can be used in this cipher.
        /////// </summary>
        /////// <param name="rotorPosition">The position to get the available rotors for.</param>
        /////// <returns>A collection of available rotors.</returns>
        //// public IEnumerable<EnigmaRotorNumber> AvailableRotors()
        //// {
        ////    return this.availableRotors;

        //// // this.availableRotors.Sort(new EnigmaRotorNumberSorter(SortOrder.Ascending));
        ////    // return new Collection<EnigmaRotorNumber>(this.availableRotors[rotorPosition].ToArray());
        //// }

        /////// <summary>
        /////// Set the rotor order for this machine.
        /////// </summary>
        /////// <param name="rotorPosition">The position to place this rotor in.</param>
        /////// <param name="rotorNumber">The rotor to put in this position.</param>
        //// public void SetRotorOrder(EnigmaRotorPosition rotorPosition, EnigmaRotorSettings rotor)
        //// {
        ////    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        ////    {
        ////        throw new ArgumentException("This position is not available.");
        ////    }

        //// // Is the rotor being set to the existing one?
        ////    if (this.rotorSettings[rotorPosition].RotorNumber == rotor.RotorNumber)
        ////    {
        ////        return;
        ////    }

        //// if (!this.availableRotors.Contains(rotor.RotorNumber))
        ////    {
        ////        throw new ArgumentException("This rotor in this position is not available.");
        ////    }

        //// this.SetRotorOrder_Private(rotorPosition, rotor);

        //// // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //// // It's all good, raise an event
        ////    this.OnSettingsChanged();
        //// }

        /////// <summary>
        /////// Set the rotor order for this machine.
        /////// </summary>
        /////// <param name="rotorPosition">The position to place this rotor in.</param>
        /////// <returns>The rotor to put in the position.</returns>
        //// public EnigmaRotorNumber GetRotorOrder(EnigmaRotorPosition rotorPosition)
        //// {
        ////    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        ////    {
        ////        throw new ArgumentException("Invalid rotor position.");
        ////    }

        //// return this.rotorSettings[rotorPosition].RotorNumber;
        //// }

        /////// <summary>
        /////// Gets the rotor's ring setting.
        /////// </summary>
        /////// <param name="rotorPosition">The rotor position for which to get the ring setting.</param>
        /////// <returns>The ring setting for the specified position.</returns>
        //// public char GetRingSetting(EnigmaRotorPosition rotorPosition)
        //// {
        ////    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        ////    {
        ////        throw new ArgumentException("This position is not allowed.");
        ////    }

        //// if (this.rotorSettings[rotorPosition].IsEmpty)
        ////    {
        ////        throw new ArgumentException("No rotor currently in this position.");
        ////    }

        //// if (!this.rotorSettings.ContainsKey(rotorPosition))
        ////    {
        ////        throw new ArgumentException("No rotor currently in this position.");
        ////    }

        //// return this.rotorSettings[rotorPosition].RingPosition;
        //// }

        /////// <summary>
        /////// Gets the rotor settings.
        /////// </summary>
        /////// <param name="rotorPosition">The rotor position for which to get the setting.</param>
        /////// <returns>The setting for the specified position.</returns>
        //// public char GetRotorSetting(EnigmaRotorPosition rotorPosition)
        //// {
        ////    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        ////    {
        ////        throw new ArgumentException("This position is not allowed.");
        ////    }

        //// if (this.Rotors[rotorPosition].IsNone)
        ////    {
        ////        throw new ArgumentException("No rotor currently in this position.");
        ////    }

        //// if (!this.Rotors.ContainsKey(rotorPosition))
        ////    {
        ////        throw new ArgumentException("No rotor currently in this position.");
        ////    }

        //// return this.rotorSettings[rotorPosition].CurrentSetting;
        //// }

        /////// <summary>
        /////// Sets the rotor settings.
        /////// </summary>
        /////// <param name="rotorPosition">The rotor position to set.</param>
        /////// <param name="letter">The letter to set this position to.</param>
        //// public void SetRotorSetting(EnigmaRotorPosition rotorPosition, char letter)
        //// {
        ////    if (!this.AllowedRotorPositions.Contains(rotorPosition))
        ////    {
        ////        throw new ArgumentException("This position is not allowed.");
        ////    }

        //// if (this.rotorSettings[rotorPosition].RotorNumber == EnigmaRotorNumber.None)
        ////    {
        ////        throw new ArgumentException("This setting is not allowed.");
        ////    }

        //// if (!EnigmaRotor.GetAllowedLetters(this.rotorSettings[rotorPosition].RotorNumber).Contains(letter))
        ////    {
        ////        throw new ArgumentException("This setting is not allowed.");
        ////    }

        //// this.SetRotorSetting_Private(rotorPosition, letter);

        //// // this.iv = EnigmaSettings.BuildIV(this.rotorSetting);
        ////    this.OnSettingsChanged();
        //// }

        /////// <summary>
        /////// Set the encryption Key.
        /////// </summary>
        /////// <param name="keyValue">The key for this cipher.</param>
        //// public void SetKey(byte[] keyValue)
        //// {
        ////    //if (EnigmaSettings.BuildKey(this.Model, this.ReflectorNumber, this.Rotors, this.Plugboard) == keyValue)
        ////    //{
        ////    //    return;
        ////    //}

        //// EnigmaSettings enigmaKey = ParseEnigmaKey(keyValue);

        //// // Model
        ////    this.Model = enigmaKey.Model;
        ////    // this.SetEnigmaModel();

        //// // Rotor Order
        ////    this.Rotors = enigmaKey.Rotors;

        //// //foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> rotor in enigmaKey.RotorSettings)
        ////    //{
        ////    //    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorPosition), rotor.Key));
        ////    //    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorNumber), rotor.Value.RotorNumber));
        ////    //    this.Rotors[rotor.Key] = rotor.Value;
        ////    //}

        //// // Plugboard
        ////    // this.Plugboard.SetSubstitutions(enigmaKey.PlugboardPairs);
        ////    this.Plugboard = enigmaKey.Plugboard;

        //// // this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

        //// // No need to build IV?
        ////    this.NotifyPropertyChanged();
        //// }

        /////// <summary>
        /////// Get the encryption Key.
        /////// </summary>
        /////// <returns>The encryption key.</returns>
        //// public byte[] GetKey()
        //// {
        ////    Contract.Ensures(Contract.Result<byte[]>() != null);

        //// byte[] key = EnigmaSettings.BuildKey(this.Model, this.ReflectorNumber, this.Rotors, this.Plugboard);

        //// Contract.Assert(key != null);
        //// // this.SetRotorSetting_Private(rotorPosition, allowedLetters[0]);
        ////        }
        ////    }
        //// }

        ////internal static int GetIvLength()
        ////{
        ////    return 5;
        ////}

        internal static EnigmaSettings ParseKey(byte[] key)
        {
            // Example:
            // model|reflector|rotors|ring|plugboard
            EnigmaSettings settings = new EnigmaSettings();

            char[] tempKey = Encoding.Unicode.GetChars(key);
            string keyString = new string(tempKey);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.");
            }

            // Reflector
            settings.ReflectorNumber = (EnigmaReflectorNumber)Enum.Parse(typeof(EnigmaReflectorNumber), parts[0]);
            ////if (!GetAllowed(settings._model).Contains(settings.ReflectorNumber))
            ////{
            ////    throw new ArgumentException("This reflector is not available.");
            ////}

            // Rotor Order
            string[] rotors = parts[1].Split(new char[] { KeyDelimiter });
            if (rotors.Length <= 0)
            {
                throw new ArgumentException("No rotors specified.");
            }

            int rotorPositionsCount = EnigmaRotorSettings.RotorPositions.Count();

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
            string[] rings = parts[2].Split(new char[] { KeyDelimiter });

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

            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();

            for (int i = 0; i < rotors.Length; i++)
            {
                if (string.IsNullOrEmpty(rotors[i]) || rotors[i].Contains("\0"))
                {
                    throw new ArgumentException("Null or empty rotor specified.");
                }

                if (!Enum.TryParse<EnigmaRotorPosition>(i.ToString(), out EnigmaRotorPosition rotorPosition))
                {
                    throw new ArgumentException($"Invalid rotor position {i}.");
                }

                if (!Enum.TryParse<EnigmaRotorNumber>(rotors[i], out EnigmaRotorNumber rotorNumber))
                {
                    throw new ArgumentException($"Invalid rotor number {rotors[i]}.");
                }

                if (!rotorSettings.AvailableRotors.Contains(rotorNumber))
                {
                    throw new ArgumentException("This rotor in this position is not available.");
                }

                EnigmaRotor enigmaRotor = new EnigmaRotor(rotorNumber);
                if (!CharacterSet.Contains(rings[i][0]))
                {
                    throw new ArgumentException("This ring position is invalid.");
                }

                enigmaRotor.RingPosition = rings[i][0];

                // enigmaRotor.CurrentSetting = 'A';
                rotorSettings[rotorPosition] = enigmaRotor;
            }

            settings.Rotors = rotorSettings;

            // Plugboard
            string plugs = parts[3];

            //// if (string.IsNullOrEmpty(plugs) || plugs.Contains("\0"))
            //// {
            ////    throw new ArgumentException("Null or empty plugs specified.");
            //// }

            settings.Plugboard = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes($"{CharacterSet}|{plugs}|{true}"));

            return settings;
        }

        internal void AdvanceRotor(EnigmaRotorPosition rotorPosition, char currentSetting)
        {
            Rotors[rotorPosition].CurrentSetting = currentSetting;

            // this.SetRotorSetting(rotorPosition, currentSetting);
        }

        private static byte[] BuildIV(EnigmaRotorSettings rotorSettings)
        {
            // Example:
            // G M Y
            byte[] result = Encoding.Unicode.GetBytes(rotorSettings.SettingKey());

            return result;
        }

        private static byte[] BuildKey(EnigmaReflectorNumber reflector, EnigmaRotorSettings rotors, MonoAlphabeticSettings plugboard)
        {
            // Example:
            // "reflector|rotors|ring|plugboard"
            // "B|III II I|C B A|DN GR IS KC QX TM PV HY FW BJ"
            StringBuilder key = new StringBuilder();

            // Reflector
            key.Append(reflector.ToString());
            key.Append(KeySeperator);

            // Rotor order
            key.Append(rotors.RotorOrderKey());
            key.Append(KeySeperator);

            // Ring setting
            key.Append(rotors.RingKey());
            key.Append(KeySeperator);

            // Plugboard
            IReadOnlyDictionary<char, char> substitutions = plugboard.Substitutions();

            foreach (KeyValuePair<char, char> pair in substitutions)
            {
                key.Append(pair.Key);
                key.Append(pair.Value);
                key.Append(" ");
            }

            if (substitutions.Count > 0
                && key.Length > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return Encoding.Unicode.GetBytes(key.ToString());
        }

        //// return allowedRotorPositions;
        //// }

        ///// <summary>
        ///// Gets all the allowed rotors for a given Enigma machine type.
        ///// </summary>
        ///// <param name="model">The specified Enigma machine type.</param>
        ///// <returns>All the allowed rotors for the machine type and position.</returns>
        // private static List<EnigmaRotorNumber> GetAllowedRotors(EnigmaModel model)
        // {
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaModel), model));

        // switch (model)
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

        // case EnigmaModel.M3:
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

        // case EnigmaModel.M4:
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

        // default:
        //            {
        //                throw new ArgumentException("Unknown Enigma model.");
        //            }
        //    }
        // }

        ///// <summary>
        ///// Gets the allowed rotors for a given rotor position in a given Enigma machine type.
        ///// </summary>
        ///// <param name="model">The specified Enigma machine type.</param>
        ///// <param name="rotorPosition">The specified rotor position.</param>
        ///// <returns>The allowed rotors for the machine type and position.</returns>
        // internal static List<EnigmaRotorNumber> GetAllowedRotors(EnigmaModel model, EnigmaRotorPosition rotorPosition)
        // {
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaModel), model));
        //    Contract.Requires(Enum.IsDefined(typeof(EnigmaRotorPosition), rotorPosition));

        // Contract.Requires(EnigmaSettings.GetAllowedRotorPositions(model).Contains(rotorPosition));

        // List<EnigmaRotorNumber> allowedRotors = new List<EnigmaRotorNumber>();

        // allowedRotors.Add(EnigmaRotorNumber.None);

        // switch (model)
        //    {
        //        case EnigmaModel.Military:
        //            {
        //                allowedRotors.Add(EnigmaRotorNumber.One);
        //                allowedRotors.Add(EnigmaRotorNumber.Two);
        //                allowedRotors.Add(EnigmaRotorNumber.Three);
        //                allowedRotors.Add(EnigmaRotorNumber.Four);
        //                allowedRotors.Add(EnigmaRotorNumber.Five);

        // return allowedRotors;
        //            }

        // case EnigmaModel.M3:
        //            {
        //                allowedRotors.Add(EnigmaRotorNumber.One);
        //                allowedRotors.Add(EnigmaRotorNumber.Two);
        //                allowedRotors.Add(EnigmaRotorNumber.Three);
        //                allowedRotors.Add(EnigmaRotorNumber.Four);
        //                allowedRotors.Add(EnigmaRotorNumber.Five);
        //                allowedRotors.Add(EnigmaRotorNumber.Six);
        //                allowedRotors.Add(EnigmaRotorNumber.Seven);
        //                allowedRotors.Add(EnigmaRotorNumber.Eight);

        // return allowedRotors;
        //            }

        // case EnigmaModel.M4:
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

        // default:
        //            {
        //                throw new ArgumentException("Unknown Enigma model.");
        //            }
        //    }
        // }

        ///// <summary>
        ///// Returns the available rotors for a given Enigma model and given position.
        ///// </summary>
        ///// <param name="model">The specified Enigma model.</param>
        ///// <param name="rotorsByPosition">The rotors currently in the positions.</param>
        ///// <param name="rotorPosition">Which position to get the availablity for.</param>
        ///// <returns>The available rotors for a given Enigma model and given position.</returns>
        // private static List<EnigmaRotorNumber> GetAvailableRotors(EnigmaModel model, SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSettings, EnigmaRotorPosition rotorPosition)
        // {
        //    Contract.Requires(rotorSettings != null);
        //    Contract.Assert(EnigmaSettings.GetAllowedRotorPositions(model).Contains(rotorPosition));

        // List<EnigmaRotorNumber> allowedRotors = GetAllowedRotors(model, rotorPosition);

        // foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorSettings> rotorSetting in rotorSettings)
        //    {
        //        Contract.Assert(EnigmaSettings.GetAllowedRotorPositions(model).Contains(rotorSetting.Key));
        //        //Contract.Assert(EnigmaSettings.GetAllowedRotors(model, rotorSettings[rotorSettings].RingPosition).Contains(rotorByPosition.Value));

        // if (rotorSetting.Value.RotorNumber != EnigmaRotorNumber.None)
        //        {
        //            if (allowedRotors.Contains(rotorSetting.Value.RotorNumber))
        //            {
        //                allowedRotors.Remove(rotorSetting.Value.RotorNumber);
        //            }
        //        }
        //    }

        // return allowedRotors;
        // }

        //// internal static Collection<EnigmaRotorPosition> GetAllowedRotorPositions(EnigmaModel model)
        //// {
        ////    Collection<EnigmaRotorPosition> allowedRotorPositions = new Collection<EnigmaRotorPosition>();

        //// switch (model)
        ////    {
        ////        case EnigmaModel.Military:
        ////            {
        ////                allowedRotorPositions.Add(EnigmaRotorPosition.Fastest);
        ////                allowedRotorPositions.Add(EnigmaRotorPosition.Second);
        ////                allowedRotorPositions.Add(EnigmaRotorPosition.Third);
        ////                break;
        ////            }
        ////        case EnigmaModel.M3:
        ////        case EnigmaModel.M4:
        ////            {
        ////                allowedRotorPositions.Add(EnigmaRotorPosition.Fastest);
        ////                allowedRotorPositions.Add(EnigmaRotorPosition.Second);
        ////                allowedRotorPositions.Add(EnigmaRotorPosition.Third);
        ////                allowedRotorPositions.Add(EnigmaRotorPosition.Forth);
        ////                break;
        ////            }

        //// default:
        ////            {
        ////                throw new ArgumentException("Unknown Enigma model.");
        ////            }
        ////    }

        private static (EnigmaReflectorNumber, EnigmaRotorSettings, MonoAlphabeticSettings) GetSettings(byte[] key, byte[] iv)
        {
            _ = key;
            _ = iv;

            EnigmaReflectorNumber reflector = EnigmaReflectorNumber.B;
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            MonoAlphabeticSettings plugboard = new MonoAlphabeticSettings();

            ////// if (EnigmaSettings.BuildKey(this.Model, this.ReflectorNumber, this.Rotors, this.Plugboard) == keyValue)
            ////// {
            //////    return;
            ////// }
            ////EnigmaSettings settings = ParseKey(value);

            ////// Model
            ////model = settings.Model;

            ////// this.SetEnigmaModel();

            ////// Rotor Order
            ////Rotors = settings.Rotors;

            ////// foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> rotor in enigmaKey.RotorSettings)
            ////// {
            //////    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorPosition), rotor.Key));
            //////    Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorNumber), rotor.Value.RotorNumber));
            //////    this.Rotors[rotor.Key] = rotor.Value;
            ////// }

            ////// Plugboard
            ////// this.Plugboard.SetSubstitutions(enigmaKey.PlugboardPairs);
            ////Plugboard = settings.Plugboard;

            ////// this.key = EnigmaSettings.BuildKey(this.Model, this.rotorsByPosition, this.Plugboard);

            return (reflector, rotorSettings, plugboard);
        }
    }
}