// <copyright file="EnigmaSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
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

        private const string DefaultCharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// The number of fields in the key.
        /// </summary>
        private const int KeyParts = 4;

        /// <summary>
        /// The seperator between key fields.
        /// </summary>
        private const char KeySeperator = '|';

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSettings"/> class.
        /// </summary>
        public EnigmaSettings()
            : this(EnigmaReflectorNumber.B, new EnigmaRotorSettings(), new MonoAlphabeticSettings(Encoding.Unicode.GetBytes($"{DefaultCharacterSet}||True")))
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

        /// <summary>
        /// Gets the character set.
        /// </summary>
        /// <value>The character set.</value>
        public static IList<char> CharacterSet
        {
            get => DefaultCharacterSet.ToCharArray();
        }

        /// <inheritdoc/>
        public override IEnumerable<byte> IV
        {
            get
            {
                // Example:
                // G M Y
                byte[] result = Encoding.Unicode.GetBytes(Rotors.SettingKey());

                return result;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<byte> Key
        {
            get
            {
                // Example:
                // "reflector|rotors|ring|plugboard"
                // "B|III II I|C B A|DN GR IS KC QX TM PV HY FW BJ"
                StringBuilder key = new StringBuilder();

                // Reflector
                key.Append(ReflectorNumber.ToString());
                key.Append(KeySeperator);

                // Rotor order
                key.Append(Rotors.RotorOrderKey());
                key.Append(KeySeperator);

                // Ring setting
                key.Append(Rotors.RingKey());
                key.Append(KeySeperator);

                // Plugboard
                IReadOnlyDictionary<char, char> substitutions = Plugboard.Substitutions();

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
        }

        /// <summary>
        /// Gets or sets the plugboard settings.
        /// </summary>
        public MonoAlphabeticSettings Plugboard { get; set; }

        /// <summary>
        /// Gets the reflector being used.
        /// </summary>
        public EnigmaReflectorNumber ReflectorNumber { get; private set; }

        /// <summary>
        /// Gets the rotors.
        /// </summary>
        public EnigmaRotorSettings Rotors { get; private set; }

        internal void AdvanceRotor(EnigmaRotorPosition rotorPosition, char currentSetting)
        {
            Rotors[rotorPosition].CurrentSetting = currentSetting;

            // this.SetRotorSetting(rotorPosition, currentSetting);
        }

        private static (EnigmaReflectorNumber, EnigmaRotorSettings, MonoAlphabeticSettings) GetSettings(byte[] key, byte[] iv)
        {
            string keyString = Encoding.Unicode.GetString(key);
            string ivString = Encoding.Unicode.GetString(iv);

            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.");
            }

            // Reflector
            EnigmaReflectorNumber reflector = (EnigmaReflectorNumber)Enum.Parse(typeof(EnigmaReflectorNumber), parts[0]);

            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings(parts[1], parts[2], ivString);

            // Plugboard
            string plugs = parts[3];

            MonoAlphabeticSettings plugboard = new MonoAlphabeticSettings(Encoding.Unicode.GetBytes($"{DefaultCharacterSet}|{plugs}|{true}"));

            return (reflector, rotorSettings, plugboard);
        }
    }
}