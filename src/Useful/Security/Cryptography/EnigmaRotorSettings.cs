// <copyright file="EnigmaRotorSettings.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class EnigmaRotorSettings : INotifyPropertyChanged
    {
        private EnigmaModel _model;
        private List<EnigmaRotorNumber> _availableRotors;
        private IDictionary<EnigmaRotorPosition, EnigmaRotor> _list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();

        private EnigmaRotorSettings(EnigmaModel model)
        {
            _model = model;

            AllowedRotorPositions = EnigmaRotorSettings.GetAllowedRotorPositions(model);
            _availableRotors = EnigmaRotorSettings.GetAllowedRotors(model);

            _list = new Dictionary<EnigmaRotorPosition, EnigmaRotor>(AllowedRotorPositions.ToList().Count);
            foreach (EnigmaRotorPosition position in AllowedRotorPositions)
            {
                _list[position] = EnigmaRotor.Create(EnigmaRotorNumber.None);
            }
        }

        public static EnigmaRotorSettings Create(EnigmaModel model)
        {
            return new EnigmaRotorSettings(model);
        }

        public int Count => _list.Count;

        /// <summary>
        /// Gets the allowed rotor positions.
        /// </summary>
        public IEnumerable<EnigmaRotorPosition> AllowedRotorPositions { get; private set; }

        public IEnumerable<EnigmaRotorNumber> AvailableRotors
        {
            get
            {
                return _availableRotors;
            }
        }

        /// <summary>
        /// Sets the rotor settings.
        /// </summary>
        /// <param name="position">The rotor position to set.</param>
        /// <returns>The rotor to set in this position.</returns>
        public EnigmaRotor this[EnigmaRotorPosition position]
        {
            get
            {
                Contract.Requires(AllowedRotorPositions.Contains(position));

                return _list[position];
            }

            set
            {
                Contract.Requires(AllowedRotorPositions.Contains(position));
                Contract.Requires(this[position].RotorNumber != value.RotorNumber);

                _list[position] = value;

                NotifyPropertyChanged();

                // Correct the available rotors list
                // Never remove the None rotor from the list
                if (value.RotorNumber != EnigmaRotorNumber.None)
                {
                    if (_availableRotors.Contains(value.RotorNumber))
                    {
                        _availableRotors.Remove(value.RotorNumber);
                        NotifyPropertyChanged(nameof(AvailableRotors));
                    }
                }
            }
        }

        /// <summary>
        /// Gets all the allowed rotors for a given Enigma machine type.
        /// </summary>
        /// <param name="model">The specified Enigma machine type.</param>
        /// <returns>All the allowed rotors for the machine type and position.</returns>
        private static List<EnigmaRotorNumber> GetAllowedRotors(EnigmaModel model)
        {
            switch (model)
            {
                case EnigmaModel.Military:
                    {
                        return new List<EnigmaRotorNumber>(6) {
                            EnigmaRotorNumber.None,
                            EnigmaRotorNumber.One,
                            EnigmaRotorNumber.Two,
                            EnigmaRotorNumber.Three,
                            EnigmaRotorNumber.Four,
                            EnigmaRotorNumber.Five,
                        };
                    }

                case EnigmaModel.M3:
                    {
                        return new List<EnigmaRotorNumber>(9) {
                            EnigmaRotorNumber.None,
                            EnigmaRotorNumber.One,
                            EnigmaRotorNumber.Two,
                            EnigmaRotorNumber.Three,
                            EnigmaRotorNumber.Four,
                            EnigmaRotorNumber.Five,
                            EnigmaRotorNumber.Six,
                            EnigmaRotorNumber.Seven,
                            EnigmaRotorNumber.Eight,
                        };
                    }

                case EnigmaModel.M4:
                    {
                        return new List<EnigmaRotorNumber>(11) {
                            EnigmaRotorNumber.None,
                            EnigmaRotorNumber.One,
                            EnigmaRotorNumber.Two,
                            EnigmaRotorNumber.Three,
                            EnigmaRotorNumber.Four,
                            EnigmaRotorNumber.Five,
                            EnigmaRotorNumber.Six,
                            EnigmaRotorNumber.Seven,
                            EnigmaRotorNumber.Eight,
                            EnigmaRotorNumber.Beta,
                            EnigmaRotorNumber.Gamma,
                        };
                    }

                default:
                    {
                        throw new ArgumentException("Unknown Enigma model.");
                    }
            }
        }

        /// <summary>
        /// Gets the allowed rotors for a given rotor position in a given Enigma machine type.
        /// </summary>
        /// <param name="model">The specified Enigma machine type.</param>
        /// <param name="rotorPosition">The specified rotor position.</param>
        /// <returns>The allowed rotors for the machine type and position.</returns>
        internal static List<EnigmaRotorNumber> GetAllowedRotors(EnigmaModel model, EnigmaRotorPosition rotorPosition)
        {
            Contract.Requires(GetAllowedRotorPositions(model).Contains(rotorPosition));

            List<EnigmaRotorNumber> allowedRotors = new List<EnigmaRotorNumber>
            {
                EnigmaRotorNumber.None,
            };

            switch (model)
            {
                case EnigmaModel.Military:
                    {
                        allowedRotors.Add(EnigmaRotorNumber.One);
                        allowedRotors.Add(EnigmaRotorNumber.Two);
                        allowedRotors.Add(EnigmaRotorNumber.Three);
                        allowedRotors.Add(EnigmaRotorNumber.Four);
                        allowedRotors.Add(EnigmaRotorNumber.Five);

                        return allowedRotors;
                    }

                case EnigmaModel.M3:
                    {
                        allowedRotors.Add(EnigmaRotorNumber.One);
                        allowedRotors.Add(EnigmaRotorNumber.Two);
                        allowedRotors.Add(EnigmaRotorNumber.Three);
                        allowedRotors.Add(EnigmaRotorNumber.Four);
                        allowedRotors.Add(EnigmaRotorNumber.Five);
                        allowedRotors.Add(EnigmaRotorNumber.Six);
                        allowedRotors.Add(EnigmaRotorNumber.Seven);
                        allowedRotors.Add(EnigmaRotorNumber.Eight);

                        return allowedRotors;
                    }

                case EnigmaModel.M4:
                    {
                        if (rotorPosition == EnigmaRotorPosition.Fastest
                            || rotorPosition == EnigmaRotorPosition.Second
                            || rotorPosition == EnigmaRotorPosition.Third)
                        {
                            allowedRotors.Add(EnigmaRotorNumber.One);
                            allowedRotors.Add(EnigmaRotorNumber.Two);
                            allowedRotors.Add(EnigmaRotorNumber.Three);
                            allowedRotors.Add(EnigmaRotorNumber.Four);
                            allowedRotors.Add(EnigmaRotorNumber.Five);
                            allowedRotors.Add(EnigmaRotorNumber.Six);
                            allowedRotors.Add(EnigmaRotorNumber.Seven);
                            allowedRotors.Add(EnigmaRotorNumber.Eight);
                        }
                        else if (rotorPosition == EnigmaRotorPosition.Forth)
                        {
                            allowedRotors.Add(EnigmaRotorNumber.Beta);
                            allowedRotors.Add(EnigmaRotorNumber.Gamma);
                        }

                        return allowedRotors;
                    }

                default:
                    {
                        throw new ArgumentException("Unknown Enigma model.");
                    }
            }
        }

        /// <summary>
        /// Returns the available rotors for a given Enigma model and given position.
        /// </summary>
        /// <param name="model">The specified Enigma model.</param>
        /// <param name="rotorsByPosition">The rotors currently in the positions.</param>
        /// <param name="rotorPosition">Which position to get the availablity for.</param>
        /// <returns>The available rotors for a given Enigma model and given position.</returns>
        public List<EnigmaRotorNumber> GetAvailableRotors(EnigmaRotorPosition rotorPosition)
        {
            Contract.Assert(AllowedRotorPositions.Contains(rotorPosition));

            List<EnigmaRotorNumber> availableRotors = GetAllowedRotors(_model, rotorPosition);

            foreach (EnigmaRotorPosition position in AllowedRotorPositions)
            {
                // Contract.Assert(EnigmaSettings.GetAllowedRotorPositions(model).Contains(rotorSetting.Key));
                // Contract.Assert(EnigmaSettings.GetAllowedRotors(model, rotorSettings[rotorSettings].RingPosition).Contains(rotorByPosition.Value));
                if (_list[position].RotorNumber != EnigmaRotorNumber.None)
                {
                    if (availableRotors.Contains(_list[position].RotorNumber))
                    {
                        availableRotors.Remove(_list[position].RotorNumber);
                    }
                }
            }

            return availableRotors;
        }

        public string GetOrderKey()
        {
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _list.Reverse().ToArray())
            {
                // Contract.Assume(Enum.IsDefined(typeof(EnigmaRotorNumber), position.Value.RotorNumber));
                key.Append(EnigmaUINameConverter.Convert(position.Value.RotorNumber));
                key.Append(EnigmaSettings.KeyDelimiter);
            }

            if (_list.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        public string GetRingKey()
        {
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _list.Reverse().ToArray())
            {
                key.Append(position.Value.RingPosition);
                key.Append(EnigmaSettings.KeyDelimiter);
            }

            if (_list.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        public string GetSettingKey()
        {
            StringBuilder key = new StringBuilder();

            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotor> position in _list.Reverse().ToArray())
            {
                key.Append(position.Value.CurrentSetting);
                key.Append(EnigmaSettings.KeyDelimiter);
            }

            if (_list.Count > 0)
            {
                key.Remove(key.Length - 1, 1);
            }

            return key.ToString();
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="rotorOrder"></param>
        ///// <returns></returns>
        // private static EnigmaRotorSettings GetDefaultRotorSettings(EnigmaRotorSettings rotorOrder)
        // {
        //    Contract.Requires(rotorOrder != null);

        // SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings> rotorSetting = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorSettings>();

        // foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorSettings> rotor in rotorOrder)
        //    {
        //        Collection<char> letters = EnigmaRotor.GetAllowedLetters(rotor.Value.RotorNumber);
        //        EnigmaRotorSettings setting = new EnigmaRotorSettings(rotor.Value.RotorNumber, letters[0], letters[0]);
        //        rotorSetting.Add(rotor.Key, setting);
        //    }

        // return rotorSetting;
        // }
        public static EnigmaRotorSettings GetDefault(EnigmaModel model)
        {
            EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.Create(model);
            rotorSettings[EnigmaRotorPosition.Fastest] = EnigmaRotor.Create(EnigmaRotorNumber.Three);
            rotorSettings[EnigmaRotorPosition.Second] = EnigmaRotor.Create(EnigmaRotorNumber.Two);
            rotorSettings[EnigmaRotorPosition.Third] = EnigmaRotor.Create(EnigmaRotorNumber.One);
            switch (model)
            {
                case EnigmaModel.Military:
                    break;
                case EnigmaModel.M3:
                case EnigmaModel.M4:
                    rotorSettings[EnigmaRotorPosition.Forth] = EnigmaRotor.Create(EnigmaRotorNumber.Beta);
                    break;
                default:
                    throw new ArgumentException("Unknown Enigma model.");
            }

            return rotorSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static EnigmaRotorSettings GetRandom(EnigmaModel model)
        {
            Random rnd = new Random();
            int nextRandomNumber;
            EnigmaRotorSettings rotorSettings = EnigmaRotorSettings.Create(model);

            Collection<EnigmaRotorPosition> allowedRotorPositions = GetAllowedRotorPositions(model);

            List<EnigmaRotorNumber> availableRotorNumbers;
            foreach (EnigmaRotorPosition rotorPosition in allowedRotorPositions)
            {
                availableRotorNumbers = rotorSettings.GetAvailableRotors(rotorPosition);
                if (availableRotorNumbers.Contains(EnigmaRotorNumber.None))
                {
                    availableRotorNumbers.Remove(EnigmaRotorNumber.None);
                }

                nextRandomNumber = rnd.Next(0, availableRotorNumbers.Count);

                rotorSettings[rotorPosition] = EnigmaRotor.GetRandom(availableRotorNumbers[nextRandomNumber]);
            }

            return rotorSettings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Pure]
        public static Collection<EnigmaRotorPosition> GetAllowedRotorPositions(EnigmaModel model)
        {
            Collection<EnigmaRotorPosition> allowedRotorPositions = new Collection<EnigmaRotorPosition>();

            switch (model)
            {
                case EnigmaModel.Military:
                    {
                        allowedRotorPositions.Add(EnigmaRotorPosition.Fastest);
                        allowedRotorPositions.Add(EnigmaRotorPosition.Second);
                        allowedRotorPositions.Add(EnigmaRotorPosition.Third);
                        break;
                    }

                case EnigmaModel.M3:
                case EnigmaModel.M4:
                    {
                        allowedRotorPositions.Add(EnigmaRotorPosition.Fastest);
                        allowedRotorPositions.Add(EnigmaRotorPosition.Second);
                        allowedRotorPositions.Add(EnigmaRotorPosition.Third);
                        allowedRotorPositions.Add(EnigmaRotorPosition.Forth);
                        break;
                    }

                default:
                    {
                        throw new ArgumentException("Unknown Enigma model.");
                    }
            }

            return allowedRotorPositions;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
            {
                return;
            }

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}