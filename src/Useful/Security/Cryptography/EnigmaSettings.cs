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

        /// <summary>
        /// The number of fields in the key.
        /// </summary>
        private const int KeyParts = 4;

        /// <summary>
        /// The seperator between key fields.
        /// </summary>
        private const char KeySeperator = '|';

        private const string _defaultKey = "B|I II III|A A A|";

        private const string _defaultIv = "A A A";

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

        internal void AdvanceRotor(EnigmaRotorPosition rotorPosition, char currentSetting)
        {
            Rotors[rotorPosition].CurrentSetting = currentSetting;

            // this.SetRotorSetting(rotorPosition, currentSetting);
        }

        private static EnigmaSettings ParseKey(byte[] key)
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

        private static (EnigmaReflectorNumber, EnigmaRotorSettings, MonoAlphabeticSettings) GetSettings(byte[] key, byte[] iv)
        {
            _ = iv;

            char[] tempKey = Encoding.Unicode.GetChars(key);
            string keyString = new string(tempKey);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.");
            }

            // Reflector
            EnigmaReflectorNumber reflector = (EnigmaReflectorNumber)Enum.Parse(typeof(EnigmaReflectorNumber), parts[0]);
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

             // Plugboard
            string plugs = parts[3];

            //// if (string.IsNullOrEmpty(plugs) || plugs.Contains("\0"))
            //// {
            ////    throw new ArgumentException("Null or empty plugs specified.");
            //// }

            MonoAlphabeticSettings plugboard = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes($"{CharacterSet}|{plugs}|{true}"));

            return (reflector, rotorSettings, plugboard);
        }
    }
}