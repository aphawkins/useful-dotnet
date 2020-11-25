// <copyright file="EnigmaSymmetric.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Simulates the Enigma encoding machine.
    /// </summary>
    public sealed class EnigmaSymmetric : SymmetricAlgorithm
    {
        /// <summary>
        /// The seperator between values in a key field.
        /// </summary>
        internal const char KeyDelimiter = ' ';

        /// <summary>
        /// The number of fields in the key.
        /// </summary>
        private const int KeyParts = 4;

        /// <summary>
        /// The seperator between key fields.
        /// </summary>
        private const char KeySeperator = '|';

        /// <summary>
        /// The encoding used by this cipher.
        /// </summary>
        private static readonly Encoding Encoding = new UnicodeEncoding(false, false);

        private readonly Enigma _algorithm;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaSymmetric"/> class.
        /// </summary>
        public EnigmaSymmetric()
        {
            _algorithm = new Enigma(new EnigmaSettings());
            Reset();
        }

        /// <inheritdoc />
        public override byte[] Key
        {
            // Example:
            // "reflector|rotors|ring|plugboard"
            // "B|III II I|03 02 01|DN GR IS KC QX TM PV HY FW BJ"
            get
            {
                StringBuilder key = new();

                // Reflector
                key.Append(_algorithm.Settings.ReflectorNumber.ToString());
                key.Append(KeySeperator);

                // Rotor order
                key.Append(_algorithm.Settings.Rotors.RotorOrderKey());
                key.Append(KeySeperator);

                // Ring setting
                key.Append(_algorithm.Settings.Rotors.RotorRingKey());
                key.Append(KeySeperator);

                // Plugboard
                key.Append(PlugboardString(_algorithm.Settings.Plugboard));

                return Encoding.Unicode.GetBytes(key.ToString());
            }

            set
            {
                try
                {
                    _algorithm.Settings = GetSettingsKey(value);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error parsing Key.", nameof(Key), ex);
                }

                base.Key = value;
            }
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get
            {
                // Example:
                // G M Y
                byte[] result = Encoding.Unicode.GetBytes(_algorithm.Settings.Rotors.RotorSettingKey());
                return result;
            }

            set
            {
                try
                {
                    _algorithm.Settings = GetSettingsIv(_algorithm.Settings, value);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error parsing IV.", nameof(IV), ex);
                }

                base.IV = value;
            }
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            Key = rgbKey;
            IV = rgbIV ?? Array.Empty<byte>();
            return new ClassicalSymmetricTransform(_algorithm, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[]? rgbIV)
        {
            Key = rgbKey;
            IV = rgbIV ?? Array.Empty<byte>();
            return new ClassicalSymmetricTransform(_algorithm, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc />
        public override void GenerateIV()
        {
            IEnigmaSettings settings = EnigmaSettingsGenerator.GenerateIV(_algorithm.Settings);
            IVValue = Encoding.GetBytes(settings.Rotors.RotorSettingKey());
            IV = IVValue;
        }

        /// <inheritdoc />
        public override void GenerateKey()
        {
            IEnigmaSettings settings = EnigmaSettingsGenerator.GenerateKey();
            string key = settings.ReflectorNumber.ToString() + KeySeperator
                + settings.Rotors.RotorOrderKey() + KeySeperator
                + settings.Rotors.RotorRingKey() + KeySeperator
                + PlugboardString(settings.Plugboard);
            KeyValue = Encoding.GetBytes(key);
            Key = KeyValue;
        }

        /// <inheritdoc/>
        public override string ToString() => _algorithm.CipherName;

        private static IEnigmaSettings GetSettingsKey(byte[] key)
        {
            string keyString = Encoding.Unicode.GetString(key);
            string[] parts = keyString.Split(new char[] { KeySeperator }, StringSplitOptions.None);

            if (parts.Length != KeyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.", nameof(key));
            }

            EnigmaReflectorNumber reflector = ParseEnigmaReflectorNumber(parts[0]);
            IDictionary<EnigmaRotorPosition, EnigmaRotorNumber> rotorNumbers = ParseEnigmaRotorNumbers(parts[1]);
            IDictionary<EnigmaRotorPosition, int> rings = ParseEnigmaRings(parts[2]);

            IReadOnlyDictionary<EnigmaRotorPosition, EnigmaRotor> list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>
            {
                { EnigmaRotorPosition.Fastest, new EnigmaRotor(rotorNumbers[EnigmaRotorPosition.Fastest], rings[EnigmaRotorPosition.Fastest], 'A') },
                { EnigmaRotorPosition.Second, new EnigmaRotor(rotorNumbers[EnigmaRotorPosition.Second], rings[EnigmaRotorPosition.Second], 'A') },
                { EnigmaRotorPosition.Third, new EnigmaRotor(rotorNumbers[EnigmaRotorPosition.Third], rings[EnigmaRotorPosition.Third], 'A') },
            };

            EnigmaRotorSettings rotors = new(list);

            EnigmaPlugboard plugboard = ParsePlugboard(parts[3]);

            return new EnigmaSettings(reflector, rotors, plugboard);
        }

        private static IEnigmaSettings GetSettingsIv(IEnigmaSettings settings, byte[] iv)
        {
            string ivString = iv != null ? Encoding.Unicode.GetString(iv) : string.Empty;

            IDictionary<EnigmaRotorPosition, char> rotorSettings = ParseEnigmaRotorSettings(ivString);

            settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = rotorSettings[EnigmaRotorPosition.Fastest];
            settings.Rotors[EnigmaRotorPosition.Second].CurrentSetting = rotorSettings[EnigmaRotorPosition.Second];
            settings.Rotors[EnigmaRotorPosition.Third].CurrentSetting = rotorSettings[EnigmaRotorPosition.Third];

            return settings;
        }

        private static EnigmaReflectorNumber ParseEnigmaReflectorNumber(string reflector)
        {
            if (reflector.Length > 1 ||
                !char.IsLetter(reflector[0]) ||
                !Enum.TryParse(reflector, out EnigmaReflectorNumber reflectorNumber))
            {
                throw new ArgumentException("Incorrect reflector.", nameof(reflector));
            }

            return reflectorNumber;
        }

        private static IDictionary<EnigmaRotorPosition, EnigmaRotorNumber> ParseEnigmaRotorNumbers(string rotorNumbers)
        {
            int rotorPositionsCount = 3;
            string[] rotors = rotorNumbers.Split(new char[] { ' ' });
            Dictionary<EnigmaRotorPosition, EnigmaRotorNumber> newRotors = new();

            if (rotors.Length <= 0)
            {
                throw new ArgumentException("No rotors specified.", nameof(rotorNumbers));
            }

            if (rotors.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rotors specified.", nameof(rotorNumbers));
            }

            if (rotors.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rotors specified.", nameof(rotorNumbers));
            }

            for (int i = 0; i < rotors.Length; i++)
            {
                string rotor = rotors.Reverse().ToList()[i];
                if (string.IsNullOrEmpty(rotor) || rotor.Contains("\0"))
                {
                    throw new ArgumentException("Null or empty rotor specified.", nameof(rotorNumbers));
                }

                if (!Enum.TryParse(rotor, out EnigmaRotorNumber rotorNumber)
                    || rotorNumber.ToString() != rotor)
                {
                    throw new ArgumentException($"Invalid rotor number {rotor}.", nameof(rotorNumbers));
                }

                newRotors.Add((EnigmaRotorPosition)i, rotorNumber);
            }

            return newRotors;
        }

        private static IDictionary<EnigmaRotorPosition, int> ParseEnigmaRings(string ringSettings)
        {
            int rotorPositionsCount = 3;
            string[] rings = ringSettings.Split(new char[] { ' ' });

            if (rings.Length <= 0)
            {
                throw new ArgumentException("No rings specified.", nameof(ringSettings));
            }

            if (rings.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rings specified.", nameof(ringSettings));
            }

            if (rings.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rings specified.", nameof(ringSettings));
            }

            if (rings[0].Length == 0)
            {
                throw new ArgumentException("No rings specified.", nameof(ringSettings));
            }

            for (int i = 0; i < rings.Length; i++)
            {
                if (rings[i].Length != 2)
                {
                    throw new ArgumentException("Ring number format incorrect.", nameof(ringSettings));
                }

                if (!int.TryParse(rings[i], out _))
                {
                    throw new ArgumentException("Ring number is not a number.", nameof(ringSettings));
                }
            }

            return new Dictionary<EnigmaRotorPosition, int>
            {
                { EnigmaRotorPosition.Fastest, int.Parse(rings[2]) },
                { EnigmaRotorPosition.Second, int.Parse(rings[1]) },
                { EnigmaRotorPosition.Third, int.Parse(rings[0]) },
            };
        }

        private static IDictionary<EnigmaRotorPosition, char> ParseEnigmaRotorSettings(string rotorSettings)
        {
            int rotorPositionsCount = 3;
            string[] rotorSetting = rotorSettings.Split(new char[] { ' ' });

            if (rotorSetting.Length <= 0)
            {
                throw new ArgumentException("No rotor settings specified.", nameof(rotorSettings));
            }

            if (rotorSetting.Length > rotorPositionsCount)
            {
                throw new ArgumentException("Too many rotor settings specified.", nameof(rotorSettings));
            }

            if (rotorSetting.Length < rotorPositionsCount)
            {
                throw new ArgumentException("Too few rotor settings specified.", nameof(rotorSettings));
            }

            if (rotorSetting[0].Length == 0)
            {
                throw new ArgumentException("No rotor settings specified.", nameof(rotorSettings));
            }

            return new Dictionary<EnigmaRotorPosition, char>
            {
                { EnigmaRotorPosition.Fastest, rotorSetting[2][0] },
                { EnigmaRotorPosition.Second, rotorSetting[1][0] },
                { EnigmaRotorPosition.Third, rotorSetting[0][0] },
            };
        }

        private static EnigmaPlugboard ParsePlugboard(string plugboard)
        {
            IDictionary<char, char> pairs = new Dictionary<char, char>();
            string[] rawPairs = plugboard.Split(new char[] { KeyDelimiter });

            // No plugs specified
            if (rawPairs.Length == 1 && rawPairs[0].Length == 0)
            {
                return new EnigmaPlugboard(pairs);
            }

            // Check for plugs made up of pairs
            foreach (string rawPair in rawPairs)
            {
                if (rawPair.Length != 2)
                {
                    throw new ArgumentException("Setting must be a pair.", nameof(plugboard));
                }

                if (pairs.ContainsKey(rawPair[0]))
                {
                    throw new ArgumentException("Setting already set.", nameof(plugboard));
                }

                pairs.Add(rawPair[0], rawPair[1]);
            }

            return new EnigmaPlugboard(pairs);
        }

        private static string PlugboardString(IEnigmaPlugboard plugboard)
        {
            StringBuilder key = new();
            IReadOnlyDictionary<char, char> substitutions = plugboard.Substitutions();

            foreach (KeyValuePair<char, char> pair in substitutions)
            {
                key.Append(pair.Key);
                key.Append(pair.Value);
                key.Append(KeyDelimiter);
            }

            if (substitutions.Count > 0
                && key.Length > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        private void Reset()
        {
            ModeValue = CipherMode.ECB;
            PaddingValue = PaddingMode.None;
            KeySizeValue = 16;
            BlockSizeValue = 16 * 5;
            FeedbackSizeValue = 16;
            LegalBlockSizesValue = new KeySizes[1];
            LegalBlockSizesValue[0] = new KeySizes(0, int.MaxValue, 16);
            LegalKeySizesValue = new KeySizes[1];
            LegalKeySizesValue[0] = new KeySizes(0, int.MaxValue, 16);
            KeyValue = Array.Empty<byte>();
            IVValue = Array.Empty<byte>();
        }
    }
}