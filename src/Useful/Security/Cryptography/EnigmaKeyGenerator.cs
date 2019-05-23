// <copyright file="EnigmaKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// Enigma key generator.
    /// </summary>
    internal class EnigmaKeyGenerator : IKeyGenerator
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <inheritdoc />
        public byte[] RandomIv()
        {
            EnigmaSettings enigmaKey = new EnigmaSettings(RandomKey(), Encoding.Unicode.GetBytes("A A A"));
            EnigmaRotorSettings rotorSettings = GetRandomRotorSettings(enigmaKey.Model);
            return Encoding.Unicode.GetBytes(rotorSettings.GetSettingKey());
        }

        /// <inheritdoc />
        public byte[] RandomKey()
        {
            // Model
            EnigmaModel model = GetRandomModel();

            // Reflector
            EnigmaReflectorNumber reflectorNumber;
            using (EnigmaReflector reflector = GetRandomReflector(model))
            {
                reflectorNumber = reflector.ReflectorNumber;
            }

            // Rotor Settings
            EnigmaRotorSettings rotorSettings = GetRandomRotorSettings(model);

            // Plugboard
            MonoAlphabeticSettings plugboard = new MonoAlphabeticSettings(new MonoAlphabeticKeyGenerator().RandomKey());

            EnigmaSettings settings = new EnigmaSettings(model, reflectorNumber, rotorSettings, plugboard);
            return settings.Key.ToArray();
        }

        private static EnigmaModel GetRandomModel()
        {
            return EnigmaModel.Military;
        }

        private static EnigmaReflector GetRandomReflector(EnigmaModel model)
        {
            Random rnd = new Random();

            IList<EnigmaReflectorNumber> reflectors = GetAllowedReflectors(model);

            int nextRandomNumber = rnd.Next(0, reflectors.Count);

            return new EnigmaReflector(reflectors[nextRandomNumber]);
        }

        private static EnigmaRotor GetRandom(EnigmaRotorNumber rotorNumber)
        {
            Random rnd = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int index1 = rnd.Next(0, letters.Length);
            int index2 = rnd.Next(0, letters.Length);

            EnigmaRotor rotor = new EnigmaRotor(rotorNumber)
            {
                RingPosition = letters[index1],
                CurrentSetting = letters[index2],
            };

            return rotor;
        }

        private static EnigmaRotorSettings GetRandomRotorSettings(EnigmaModel model)
        {
            Random rnd = new Random();
            int nextRandomNumber;
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings(model);

            ICollection<EnigmaRotorPosition> allowedRotorPositions = GetAllowedRotorPositions(model);

            IList<EnigmaRotorNumber> availableRotorNumbers;
            foreach (EnigmaRotorPosition rotorPosition in allowedRotorPositions)
            {
                availableRotorNumbers = rotorSettings.GetAvailableRotors(rotorPosition);
                if (availableRotorNumbers.Contains(EnigmaRotorNumber.None))
                {
                    availableRotorNumbers.Remove(EnigmaRotorNumber.None);
                }

                nextRandomNumber = rnd.Next(0, availableRotorNumbers.Count);

                rotorSettings[rotorPosition] = GetRandomRotor(availableRotorNumbers[nextRandomNumber]);
            }

            return rotorSettings;
        }

        private static IList<EnigmaReflectorNumber> GetAllowedReflectors(EnigmaModel model)
        {
            switch (model)
            {
                case EnigmaModel.Military:
                case EnigmaModel.M3:
                    {
                        return new List<EnigmaReflectorNumber>(2)
                        {
                            EnigmaReflectorNumber.B,
                            EnigmaReflectorNumber.C,
                        };
                    }

                case EnigmaModel.M4:
                    {
                        return new List<EnigmaReflectorNumber>(2)
                        {
                            EnigmaReflectorNumber.BThin,
                            EnigmaReflectorNumber.CThin,
                        };
                    }

                default:
                    {
                        throw new CryptographicException("Unknown Enigma model.");
                    }
            }
        }
    }
}