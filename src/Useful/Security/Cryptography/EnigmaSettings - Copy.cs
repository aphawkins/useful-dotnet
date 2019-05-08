using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Diagnostics;

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Accesses the Caesar Shift algorithm settings.
    /// </summary>
    public class EnigmaSettings : ISymmetricCipherSettings
    {
        private const char keySeperator = '|';
        private const char keyDelimiter = ' ';
        private const int keyParts = 3;

        private const char ivSeperator = ' ';

        private const bool isPlugboardSymmetric = true;
        private const string standardAlphabet = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private EnigmaModel m_model;
        private byte[] m_key;
        private byte[] m_iv;
        private MonoAlphabeticSettings m_plugboard;
        private SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber> m_rotorOrder;
        private SortedDictionary<EnigmaRotorPosition, char> m_initialRotorPosition;
        private SortedDictionary<EnigmaRotorPosition, char> m_currentRotorPosition;
        private bool m_hasReflector;
        private EnigmaReflectorNumber m_reflectorNumber;
        private int m_counter;
        private Collection<char> m_keyboardLetters;
        private CultureInfo m_culture = CultureInfo.InvariantCulture;
        private Collection<EnigmaRotorNumber> m_allowedRotors;
        private SortedDictionary<EnigmaRotorPosition, List<EnigmaRotorNumber>> m_allowedRotorsByPosition;
        private SortedDictionary<EnigmaRotorPosition, List<EnigmaRotorNumber>> m_availableRotorsByPosition;

        private MonoAlphabeticTransform plugboard;
        private Dictionary<EnigmaRotorNumber, EnigmaRotor> rotors;
        private SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber> reverseRotorOrder;
        private EnigmaReflector reflector;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public EnigmaSettings()
        {
            this.Initialize();
            this.SetEnigmaType(EnigmaModel.Military);
        }

        ///// <summary>
        ///// Initializes a new instance of this class.
        ///// </summary>
        ///// <param name="model">The type of Enigma machine this is.</param>
        ///// <param name="rotorOrder">The order in which the rotors are set.</param>
        ///// <param name="initialRotorPosition">Initial position of the rotors.</param>
        ///// <param name="plugboard">The plugboard settings.</param>
        //internal EnigmaSettings(
        //    EnigmaModel model,
        //    SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber> rotorOrder,
        //    SortedDictionary<EnigmaRotorPosition, char> initialRotorPosition,
        //    Collection<SubstitutionPair> plugboard)
        //{
        //    this.Initialize();
        //    this.SetEnigmaType(model);
        //    this.SetRotorOrder(rotorOrder);
        //    this.SetInitialRotorPosition(initialRotorPosition);
        //    this.SetPlugboard(plugboard);
        //}

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public EnigmaSettings(byte[] key, byte[] iv)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "Argument is null.");
            }
            if (iv == null)
            {
                throw new ArgumentNullException("iv", "Argument is null.");
            }
            this.Initialize();
            this.SetKey(key);
            this.SetIV(iv);
        }

        private void Initialize()
        {
            // Reset rotor order
            this.m_rotorOrder = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber>(new EnigmaRotorPositionSorter(SortOrder.Ascending));

            // Reset rotor settings
            this.m_initialRotorPosition = new SortedDictionary<EnigmaRotorPosition, char>(new EnigmaRotorPositionSorter(SortOrder.Ascending));
            this.m_currentRotorPosition = new SortedDictionary<EnigmaRotorPosition, char>(new EnigmaRotorPositionSorter(SortOrder.Ascending));

            this.m_allowedRotors = new Collection<EnigmaRotorNumber>();
            this.m_allowedRotorsByPosition = new SortedDictionary<EnigmaRotorPosition, List<EnigmaRotorNumber>>(new EnigmaRotorPositionSorter(SortOrder.Ascending));
            this.m_availableRotorsByPosition = new SortedDictionary<EnigmaRotorPosition, List<EnigmaRotorNumber>>(new EnigmaRotorPositionSorter(SortOrder.Ascending));

            // Reset the plugboard
            this.m_plugboard = new MonoAlphabeticSettings(this.m_keyboardLetters, null, isPlugboardSymmetric);

        }

        #region Plugboard

        ///// <summary>
        ///// Steckerverbindungen - Plugboard setting.
        ///// </summary>
        //private Collection<SubstitutionPair> PlugboardPairs
        //{
        //    get
        //    {
        //        Collection<SubstitutionPair> pairs = new Collection<SubstitutionPair>();

        //        for (int i = 0; i < this.m_plugboard.SubstitutionCount; i++)
        //        {
        //            char substitution = this.m_plugboard.GetSubstitution((char)(i + 'A'));
        //            if (substitution != (char)(i + 'A'))
        //            {
        //                SubstitutionPair pair = new SubstitutionPair();
        //                pair.From = (char)(i + 'A');
        //                pair.To = substitution;
        //                pairs.Add(pair);
        //            }
        //        }

        //        return pairs;
        //    }
        //}

        /// <summary>
        /// Plugboard settings.
        /// </summary>
        internal MonoAlphabeticTransform Plugboard
        {
            get
            {
                return this.plugboard;
            }
        }

        /// <summary>
        /// Steckerverbindungen - Plugboard setting.
        /// </summary>
        public void SetPlugboard(SubstitutionPair pair)
        {
            this.m_plugboard.SetSubstitution(pair);

            BuildKey();

            // It's all good, raise an event
            this.OnSettingsChanged();
        }

        /// <summary>
        /// Steckerverbindungen - Plugboard setting.
        /// </summary>
        public void SetPlugboard(Collection<SubstitutionPair> pairs)
        {
            this.m_plugboard.SetSubstitution(pairs);
        }

        #endregion

        #region Rotor Order
        /// <summary>
        /// Walzenlage - Rotor order.
        /// </summary>
        internal SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber> RotorOrder
        {
            get
            {
                Debug.Assert(this.m_rotorOrder != null);

                return this.m_rotorOrder;
            }
        }

        internal SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber> ReverseRotorOrder
        {
            get
            {
                Debug.Assert(this.reverseRotorOrder != null);

                return this.reverseRotorOrder;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorPosition"></param>
        /// <returns></returns>
        public char GetCurrentRotorPosition(EnigmaRotorPosition rotorPosition)
        {
            if (!this.m_currentRotorPosition.ContainsKey(rotorPosition))
            {
                throw new CryptographicException("Invalid rotor position.");
            }

            return this.m_currentRotorPosition[rotorPosition];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorPosition"></param>
        /// <param name="currentSetting"></param>
        /// <returns></returns>
        private void SetCurrentRotorPosition(EnigmaRotorPosition rotorPosition, char currentSetting)
        {
            this.m_currentRotorPosition[rotorPosition] = currentSetting;
            if (rotorPosition == EnigmaRotorPosition.Fastest)
            {
                this.m_counter++;
            }

            OnSettingsChanged();
        }

        internal EnigmaRotor GetRotor(EnigmaRotorNumber rotorNumber)
        {
            if (!this.rotors.ContainsKey(rotorNumber))
            {
                throw new CryptographicException("Invalid rotor position.");
            }

            return this.rotors[rotorNumber];
        }

        /// <summary>
        /// Set the rotor order for this machine.
        /// </summary>
        /// <param name="rotorPosition">The position to place this rotor in.</param>
        /// <returns>The rotor to put in the position.</returns>
        public EnigmaRotorNumber GetRotorOrder(EnigmaRotorPosition rotorPosition)
        {
            if (!this.m_rotorOrder.ContainsKey(rotorPosition))
            {
                throw new CryptographicException("Invalid rotor position.");
            }

            return this.m_rotorOrder[rotorPosition];
        }

        /// <summary>
        /// Set the rotor order for this machine.
        /// </summary>
        /// <param name="rotorPosition">The position to place this rotor in.</param>
        /// <param name="rotorNumber">The rotor to put in this position.</param>
        public void SetRotorOrder(EnigmaRotorPosition rotorPosition, EnigmaRotorNumber rotorNumber)
        {
            if (!m_rotorOrder.ContainsKey(rotorPosition))
            {
                throw new ArgumentException("This position is not available.");
            }
            if (rotorNumber == this.m_rotorOrder[rotorPosition])
            {
                return;
            }
            if (!this.m_availableRotorsByPosition[rotorPosition].Contains(rotorNumber))
            {
                throw new ArgumentException("This rotor in this position is not available.");
            }

            // TODO: Tidy this up
            #region removed
            //switch (rotorNumber)
            //{
            //    case EnigmaRotorNumber.Beta:
            //    case EnigmaRotorNumber.Gamma:
            //        {
            //            switch (rotorPosition)
            //            {
            //                case EnigmaRotorPosition.Forth:
            //                    {
            //                        if (this.m_model != EnigmaModel.NavyM4Thin)
            //                        {
            //                            throw new CryptographicException("Cannot have a Beta or Gamma rotor in anything but the Navy M4 thin Enigma machine.");
            //                        }
            //                        break;
            //                    }
            //                case EnigmaRotorPosition.Fastest:
            //                case EnigmaRotorPosition.Middle:
            //                case EnigmaRotorPosition.Slowest:
            //                    {
            //                        if (this.m_model == EnigmaModel.NavyM4Thin)
            //                        {
            //                            throw new CryptographicException("Cannot have a Beta or Gamma rotor in anything but the 4th rotor position with a Navy M4 thin Enigma machine.");
            //                        }
            //                        else
            //                        {
            //                            throw new CryptographicException("Cannot have a Beta or Gamma rotor in anything but the Navy M4 thin Enigma machine.");
            //                        }
            //                    }
            //                default:
            //                    {
            //                        throw new CryptographicException();
            //                    }
            //            }
            //            break;
            //        }
            //    case EnigmaRotorNumber.Six:
            //    case EnigmaRotorNumber.Seven:
            //    case EnigmaRotorNumber.Eight:
            //        {
            //            // Rotors only allowed in the Naval machines
            //            if (!(this.m_model == EnigmaModel.NavyM3
            //                || this.m_model == EnigmaModel.NavyM4Thin
            //                || this.m_model == EnigmaModel.NavyM4R2))
            //            {
            //                throw new CryptographicException("Cannot have a rotor VI, VII or VIII in anything but the Navy M3 or M4 Enigma machines.");
            //            }

            //            // Can't put full sized rotor in the M4 thin 4th position
            //            if (rotorPosition == EnigmaRotorPosition.Forth
            //                && this.m_model == EnigmaModel.NavyM4Thin)
            //            {
            //                throw new CryptographicException("Cannot have a rotor VI, VII or VIII in the 4th position of the M4 thin Enigma machines.");
            //            }
            //            break;
            //        }
            //    case EnigmaRotorNumber.One:
            //    case EnigmaRotorNumber.Two:
            //    case EnigmaRotorNumber.Three:
            //    case EnigmaRotorNumber.Four:
            //    case EnigmaRotorNumber.Five:
            //        {
            //            if (rotorPosition == EnigmaRotorPosition.Forth
            //                && this.m_model == EnigmaModel.NavyM4Thin)
            //            {
            //                throw new CryptographicException("Cannot have this rotor in the 4th position of the M4 thin Enigma machines.");
            //            }
            //            break;
            //        }
            //    default:
            //        {
            //            throw new CryptographicException("Unknown Enigma rotor number.");
            //        }
            //}


            //EnigmaRotorPositionSorter comparer = new EnigmaRotorPositionSorter();
            //comparer.Order = SortOrder.Ascending;
            #endregion

            EnigmaRotorNumber currentRotor = this.m_rotorOrder[rotorPosition];

            foreach (KeyValuePair<EnigmaRotorPosition, List<EnigmaRotorNumber>> availableRotors in this.m_availableRotorsByPosition)
            {
                // Leave the current rotor in its position
                if (availableRotors.Key != rotorPosition)
                {
                    if (!availableRotors.Value.Contains(currentRotor))
                    {
                        availableRotors.Value.Add(currentRotor);
                    }
                    if (rotorNumber != EnigmaRotorNumber.None)
                    {
                        availableRotors.Value.Remove(rotorNumber);
                    }
                }
            }

            // Set the rotor order
            if (this.m_rotorOrder.ContainsKey(rotorPosition))
            {
                this.m_rotorOrder[rotorPosition] = rotorNumber;
            }
            else
            {
                this.m_rotorOrder.Add(rotorPosition, rotorNumber);
            }

            if (m_currentRotorPosition[rotorPosition] == '\0')
            {
                SetInitialRotorPosition(rotorPosition, EnigmaRotor.GetAllowedLetters(rotorNumber)[0]);
            }

            BuildKey();

            // It's all good, raise an event
            this.OnSettingsChanged();
        }

        ///// <summary>
        ///// Set the rotor order for this machine.
        ///// </summary>
        ///// <param name="rotorOrders"></param>
        //private void SetRotorOrder(SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber> rotorOrders)
        //{
        //    if (rotorOrders == null)
        //    {
        //        throw new ArgumentException("Argument cannot be null", "rotorOrders");
        //    }
        //    // Ensure correct number of rotors have been specified.
        //    if (rotorOrders.Keys.Count != this.RotorPositionCount)
        //    {
        //        throw new CryptographicException("Incorrect number of rotors specified.");
        //    }

        //    // TODO: Ensure all required rotors have been specified.

        //    //// Reset rotor order
        //    //switch (this.model)
        //    //{
        //    //    case EnigmaModel.Military:
        //    //    case EnigmaModel.NavyM3:
        //    //        {
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.One);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Middle, EnigmaRotorNumber.Two);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Slowest, EnigmaRotorNumber.Three);
        //    //            break;
        //    //        }
        //    //    case EnigmaModel.NavyM4Thin:
        //    //        {
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.One);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Middle, EnigmaRotorNumber.Two);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Slowest, EnigmaRotorNumber.Three);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Forth, EnigmaRotorNumber.Beta);
        //    //            break;
        //    //        }
        //    //    case EnigmaModel.NavyM4R2:
        //    //        {
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.One);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Middle, EnigmaRotorNumber.Two);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Slowest, EnigmaRotorNumber.Three);
        //    //            this.SetRotorOrder(EnigmaRotorPosition.Forth, EnigmaRotorNumber.Four);
        //    //            break;
        //    //        }
        //    //    default:
        //    //        {
        //    //            throw new NotImplementedException();
        //    //            // break;
        //    //        }
        //    //}

        //    foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorNumber> rotorOrder in rotorOrders)
        //    {
        //        SetRotorOrder(rotorOrder.Key, rotorOrder.Value);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorPosition"></param>
        /// <param name="currentSetting"></param>
        internal void AdvanceRotor(EnigmaRotorPosition rotorPosition, char currentSetting)
        {
            SetCurrentRotorPosition(rotorPosition, currentSetting);
        }

        #endregion

        #region Rotor Initial
        /// <summary>
        /// Initial position of the rotors.
        /// </summary>
        internal SortedDictionary<EnigmaRotorPosition, char> InitialRotorPosition
        {
            get
            {
                return this.m_initialRotorPosition;
            }
        }

        /// <summary>
        /// Initial position of the rotors.
        /// </summary>
        /// <param name="rotorPosition"></param>
        /// <returns></returns>
        public char GetInitialRotorPosition(EnigmaRotorPosition rotorPosition)
        {
            if (!this.m_initialRotorPosition.ContainsKey(rotorPosition))
            {
                throw new CryptographicException("Invalid rotor position.");
            }
            return this.m_initialRotorPosition[rotorPosition];
        }
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotorPosition"></param>
        /// <param name="initialPosition"></param>
        public void SetInitialRotorPosition(EnigmaRotorPosition rotorPosition, char initialPosition)
        {
            if (this.m_initialRotorPosition.ContainsKey(rotorPosition))
            {
                if (this.m_initialRotorPosition[rotorPosition] == initialPosition &&
                    this.m_currentRotorPosition[rotorPosition] == initialPosition)
                {
                    // Nothing to set, so return
                    return;
                }
                this.m_initialRotorPosition[rotorPosition] = initialPosition;
                this.m_currentRotorPosition[rotorPosition] = initialPosition;
            }
            else
            {
                this.m_initialRotorPosition.Add(rotorPosition, initialPosition);
                this.m_currentRotorPosition.Add(rotorPosition, initialPosition);
            }

            this.BuildIV();

            this.OnSettingsChanged();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="positions"></param>
        //public void SetInitialRotorPosition(SortedDictionary<EnigmaRotorPosition, char> positions)
        //{
        //    // TODO: Ensure the position is allowed for this type of machine
        //    // TODO: Ensure the letter is valid for the given rotor in the position
        //    foreach (KeyValuePair<EnigmaRotorPosition, char> position in positions)
        //    {
        //        SetInitialRotorPosition(position.Key, position.Value);
        //    }
        //}

        #endregion

        #region Reflector
        #endregion

        #region Properties
        /// <summary>
        /// Rotor count.
        /// </summary>
        public int RotorPositionCount
        {
            get
            {
                return this.m_allowedRotorsByPosition.Count;
            }
        }

        ///// <summary>
        ///// The rotors that can be used in this cipher
        ///// </summary>
        //public Collection<EnigmaRotorNumber> AllowedRotors
        //{
        //    get
        //    {
        //        return this.m_allowedRotors;
        //    }
        //}

        /// <summary>
        /// The rotors that can be used in this cipher
        /// </summary>
        public Collection<EnigmaRotorNumber> AllowedRotors(EnigmaRotorPosition position)
        {
            if (!this.m_allowedRotorsByPosition.ContainsKey(position))
            {
                throw new CryptographicException("Invalid position");
            }

            this.m_allowedRotorsByPosition[position].Sort(new EnigmaRotorNumberSorter(SortOrder.Ascending));
            return new Collection<EnigmaRotorNumber>(this.m_allowedRotorsByPosition[position].ToArray());
        }

        /// <summary>
        /// The rotors that can be used in this cipher
        /// </summary>
        public Collection<EnigmaRotorNumber> AvailableRotors(EnigmaRotorPosition position)
        {
            if (!this.m_allowedRotorsByPosition.ContainsKey(position))
            {
                throw new CryptographicException("Invalid position");
            }
            this.m_availableRotorsByPosition[position].Sort(new EnigmaRotorNumberSorter(SortOrder.Ascending));
            return new Collection<EnigmaRotorNumber>(this.m_availableRotorsByPosition[position].ToArray());
        }

        /// <summary>
        /// How many letters have been en/deciphered so far.
        /// </summary>
        public int Counter
        {
            get
            {
                return this.m_counter;
            }
        }

        /// <summary>
        /// Does this Enigma machine have a reflector?
        /// </summary>
        public bool HasReflector
        {
            get
            {
                return this.m_hasReflector;
            }
        }

        /// <summary>
        /// The type of Enigma machine
        /// </summary>
        public EnigmaModel Model
        {
            get
            {
                return this.m_model;
            }
        }

        /// <summary>
        /// Reflector being used.
        /// </summary>
        public EnigmaReflectorNumber ReflectorNumber
        {
            get
            {
                return this.m_reflectorNumber;
            }
        }

        internal EnigmaReflector Reflector
        {
            get
            {
                return this.reflector;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Collection<char> KeyboardLetters
        {
            get
            {
                Debug.Assert(this.m_keyboardLetters != null);

                return this.m_keyboardLetters;
            }
        }
        #endregion

        private void OnSettingsChanged()
        {
            if (this.SettingsChanged != null)
            {
                this.SettingsChanged(this, new EventArgs());
            }
        }

        private void BuildIV()
        {
            // Example:
            // GMY

            StringBuilder iv = new StringBuilder();

            foreach (char position in this.m_initialRotorPosition.Values)
            {
                iv.Append(position);
            }

            this.m_iv = Encoding.Unicode.GetBytes(iv.ToString());

            Debug.Assert(this.m_iv != null);
        }

        private void BuildKey()
        {
            // Example:
            // "model | rotors | plugboard pairs"
            // "Military | IV II V | DN GR IS KC QX TM PV HY FW BJ"

            StringBuilder key = new StringBuilder();

            // Model
            key.Append(this.m_model.ToString());

            // Separator
            key.Append(keyDelimiter);
            key.Append(keySeperator);
            key.Append(keyDelimiter);

            // Rotor order
            foreach (EnigmaRotorNumber rotorNumber in this.m_rotorOrder.Values)
            {
                key.Append(EnigmaUINameConverter.Convert(rotorNumber));
                key.Append(keyDelimiter);
            }

            // Separator
            key.Append(keySeperator);

            if (this.m_plugboard.Substitutions.Count > 0)
            {
                key.Append(keyDelimiter);

                // Plugboard
                for (int i = 0; i < this.m_plugboard.Substitutions.Count; i++)
                {
                    char substitution = this.m_plugboard.GetSubstitution((char)(i + 'A'));
                    if (substitution != (char)(i + 'A'))
                    {
                        key.Append((char)(i + 'A'));
                        key.Append(substitution);
                        key.Append(keyDelimiter);
                    }
                }
            }

            this.m_key = Encoding.Unicode.GetBytes(key.ToString());

            Debug.Assert(this.m_key != null);
        }

        /// <summary>
        /// Set the Initialization Vector.
        /// </summary>
        public void SetIV(byte[] iv)
        {
            // Example:
            // G M Y

            if (iv.Length <= 0)
            {
                throw new CryptographicException("No rotors specified.");
            }

            char[] ivChars = Encoding.Unicode.GetChars(iv);

            string ivString = new string(ivChars);

            string[] parts = ivString.Split(new char[] { ivSeperator }, StringSplitOptions.None);

            if (parts.Length > this.RotorPositionCount)
            {
                throw new ArgumentException("Too many IV parts specified.");
            }
            if (parts.Length < this.RotorPositionCount)
            {
                throw new ArgumentException("Too few IV parts specified.");
            }

            for (int i = 0; i < parts.Length; i++)
            {
                SetInitialRotorPosition((EnigmaRotorPosition)Enum.Parse(typeof(EnigmaRotorPosition), i.ToString(this.m_culture)), parts[i][0]);
            }

            this.m_iv = iv;
        }

        /// <summary>
        /// Set the encryption Key.
        /// </summary>
        public void SetKey(byte[] key)
        {
            // Example:
            // model | IV II V | DN GR IS KC QX TM PV HY FW BJ

            if (key == null)
            {
                throw new ArgumentNullException("key", "Argument is null.");
            }
            
            char[] tempKey = Encoding.Unicode.GetChars(key);
            string keyString = new string(tempKey);

            string[] parts = keyString.Split(new char[] { keySeperator }, StringSplitOptions.None);

            if (parts.Length != keyParts)
            {
                throw new ArgumentException("Incorrect number of key parts.");
            }

            #region Model
            // Model
            if (string.IsNullOrEmpty(parts[0]))
            {
                throw new ArgumentException("Model has not been specified.");
            }
            this.m_model = (EnigmaModel)Enum.Parse(typeof(EnigmaModel), parts[0].Trim());
            this.SetEnigmaType(this.m_model);
            #endregion

            #region Rotor Order
            // Rotor Order
            string[] rotors = parts[1].Trim().Split(new char[] { keyDelimiter });
            if (rotors.Length <= 0)
            {
                throw new ArgumentException("No rotors specified.");
            }
            if (rotors.Length > this.RotorPositionCount)
            {
                throw new ArgumentException("Too many rotors specified.");
            }
            if (rotors.Length < this.RotorPositionCount)
            {
                throw new ArgumentException("Too few rotors specified.");
            }
            if (rotors[0].Length == 0)
            {
                throw new ArgumentException("No rotors specified.");
            }

            for (int i = 0; i < rotors.Length; i++)
            {
                EnigmaRotorPosition rotorPosition = (EnigmaRotorPosition)Enum.Parse(typeof(EnigmaRotorPosition), i.ToString(this.m_culture));
                SetRotorOrder(rotorPosition, EnigmaUINameConverter.Convert(rotors[i]));
            }

            // Rotors
            this.rotors = new Dictionary<EnigmaRotorNumber, EnigmaRotor>();
            foreach (EnigmaRotorNumber rotorNumber in this.m_rotorOrder.Values)
            {
                this.rotors.Add(rotorNumber, new EnigmaRotor(rotorNumber));
                this.rotors[rotorNumber].RotorAdvancedNotchHit += new EventHandler<EnigmaRotorEventArgs>(EnigmaSettings_RotorAdvancedNotchHit);
                this.rotors[rotorNumber].RotorAdvanced += new EventHandler<EnigmaRotorEventArgs>(EnigmaSettings_RotorAdvanced);

                this.rotors[rotorNumber].RotorReversedNotchHit += new EventHandler<EnigmaRotorEventArgs>(EnigmaSettings_RotorReversedNotchHit);
                this.rotors[rotorNumber].RotorReversed += new EventHandler<EnigmaRotorEventArgs>(EnigmaSettings_RotorReversed);
            }
            this.reverseRotorOrder = new SortedDictionary<EnigmaRotorPosition, EnigmaRotorNumber>(this.m_rotorOrder, new EnigmaRotorPositionSorter(SortOrder.Descending));

            // Initial rotor position
            if (this.m_initialRotorPosition.Count > 0)
            {
                foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorNumber> rotor in this.m_rotorOrder)
                {
                    this.rotors[rotor.Value].CurrentSetting = this.m_initialRotorPosition[rotor.Key];
                }
            }
            #endregion

            #region Plugboard
            // Plugboard
            string[] plugs = parts[2].Trim().Split(new char[] { keyDelimiter });
            Collection<SubstitutionPair> plugboardPairs;

            // No plugs specified
            if (plugs.Length == 1 && plugs[0].Length == 0)
            {
                plugboardPairs = null;
            }
            else
            {
                // Check for plugs made up of pairs
                SubstitutionPair plugboardPair;
                plugboardPairs = new Collection<SubstitutionPair>();
                List<char> uniqueLetters = new List<char>();

                foreach (string pair in plugs)
                {
                    if (pair.Length != 2)
                    {
                        throw new ArgumentException("Plug setting must be a pair.");
                    }

                    if (uniqueLetters.Contains(pair[0])
                        || uniqueLetters.Contains(pair[1]))
                    {
                       throw new ArgumentException("Plug setting must be unique.");
                    }

                    if (!(this.m_keyboardLetters.Contains(pair[0])
                        && this.m_keyboardLetters.Contains(pair[1])))
                    {
                        throw new ArgumentException("Invalid plugboard letters.");
                    }
                    uniqueLetters.Add(pair[0]);
                    uniqueLetters.Add(pair[1]);

                    plugboardPair = new SubstitutionPair();
                    plugboardPair.From = pair[0];
                    plugboardPair.To = pair[1];

                    plugboardPairs.Add(plugboardPair);
                }
            }
            this.m_plugboard = new MonoAlphabeticSettings(this.m_keyboardLetters, plugboardPairs, true);

            // Plugboard
            // TODO: specify the transform mode?
            this.plugboard = new MonoAlphabeticTransform(this.m_plugboard.GetKey(), this.m_plugboard.GetIV(), CipherTransformMode.Encrypt); // this.transformMode);

            #endregion

            this.m_key = key;

            this.OnSettingsChanged();
        }

        #region ISymmetricCipherSettings Members

        /// <summary>
        /// Get the Initialization Vector.
        /// </summary>
        /// <returns></returns>
        public byte[] GetIV()
        {
            Debug.Assert(this.m_iv != null);

            return this.m_iv;
        }

        /// <summary>
        /// Get the encryption Key.
        /// </summary>
        /// <returns></returns>
        public byte[] GetKey()
        {
            Debug.Assert(this.m_key != null);

            return this.m_key;
        }

        ///// <summary>
        ///// Resets the Key.
        ///// </summary>
        //private void ResetKey()
        //{
        //    this.counter = 0;



        //    // Reset rotor settings
        //    switch (this.model)
        //    {
        //        case EnigmaModel.Military:
        //        case EnigmaModel.NavyM3:
        //            {
        //                this.initialRotorPosition.Add(EnigmaRotorPosition.Fastest, 'A');
        //                this.initialRotorPosition.Add(EnigmaRotorPosition.Middle, 'A');
        //                this.initialRotorPosition.Add(EnigmaRotorPosition.Slowest, 'A');
        //                break;
        //            }
        //        case EnigmaModel.NavyM4Thin:
        //        case EnigmaModel.NavyM4R2:
        //            {
        //                this.initialRotorPosition.Add(EnigmaRotorPosition.Fastest, 'A');
        //                this.initialRotorPosition.Add(EnigmaRotorPosition.Middle, 'A');
        //                this.initialRotorPosition.Add(EnigmaRotorPosition.Slowest, 'A');
        //                this.initialRotorPosition.Add(EnigmaRotorPosition.Forth, 'A');
        //                break;
        //            }
        //        default:
        //            {
        //                throw new NotImplementedException();
        //                // break;
        //            }
        //    }

        //    this.BuildIV();
        //    this.BuildKey();

        //    //this.OnSettingsChanged();
        //}

        /// <summary>
        /// Raised when the settings are changed.
        /// </summary>
        public event EventHandler<EventArgs> SettingsChanged;

        /// <summary>
        /// The name of this cipher
        /// </summary>
        public string CipherName
        {
            get
            {
                return "Enigma";
            }
        }

        #endregion

        private void SetEnigmaType(EnigmaModel model)
        {
            this.m_allowedRotors.Clear();
            this.m_allowedRotorsByPosition.Clear();
            this.m_availableRotorsByPosition.Clear();

            switch (model)
            {
                case EnigmaModel.Military:
                    {
                        this.m_allowedRotors.Add(EnigmaRotorNumber.None);
                        this.m_allowedRotors.Add(EnigmaRotorNumber.One);
                        this.m_allowedRotors.Add(EnigmaRotorNumber.Two);
                        this.m_allowedRotors.Add(EnigmaRotorNumber.Three);
                        this.m_allowedRotors.Add(EnigmaRotorNumber.Four);
                        this.m_allowedRotors.Add(EnigmaRotorNumber.Five);

                        EnigmaRotorNumber[] positions = new EnigmaRotorNumber[this.m_allowedRotors.Count];
                        this.m_allowedRotors.CopyTo(positions, 0);

                        this.m_allowedRotorsByPosition.Add(EnigmaRotorPosition.Fastest, new List<EnigmaRotorNumber>(positions));
                        this.m_allowedRotorsByPosition.Add(EnigmaRotorPosition.Second, new List<EnigmaRotorNumber>(positions));
                        this.m_allowedRotorsByPosition.Add(EnigmaRotorPosition.Third, new List<EnigmaRotorNumber>(positions));

                        this.m_availableRotorsByPosition.Add(EnigmaRotorPosition.Fastest, new List<EnigmaRotorNumber>(positions));
                        this.m_availableRotorsByPosition.Add(EnigmaRotorPosition.Second, new List<EnigmaRotorNumber>(positions));
                        this.m_availableRotorsByPosition.Add(EnigmaRotorPosition.Third, new List<EnigmaRotorNumber>(positions));

                        SetEnigmaType(model, standardAlphabet, true, EnigmaReflectorNumber.ReflectorB);
                        break;
                    }
                //case EnigmaModel.NavyM3:
                //    {
                //        SetEnigmaType(standardAlphabet, 3, true, EnigmaReflectorNumber.ReflectorB);
                //        break;
                //    }
                //case EnigmaModel.NavyM4Thin:
                //    {
                //        SetEnigmaType(standardAlphabet, 4, true, EnigmaReflectorNumber.ReflectorBThin);
                //        break;
                //    }
                //case EnigmaModel.NavyM4R2:
                //    {
                //        SetEnigmaType(standardAlphabet, 4, true, EnigmaReflectorNumber.ReflectorB);
                //        break;
                //    }
                default:
                    {
                        throw new CryptographicException("Unknown Enigma model.");
                    }
                //case EnigmaModel.CommercialA:
                //case EnigmaModel.CommercialB:
                //case EnigmaModel.CommercialC:
                //case EnigmaModel.CommercialD:
                //case EnigmaModel.RadioCipherC:
                //case EnigmaModel.Intelligence:
                //case EnigmaModel.EnigmaII:
                //case EnigmaModel.SwissK:
                //case EnigmaModel.CounterMachine:
                //case EnigmaModel.Japanese:
                //    {
                //        throw new NotImplementedException();
                //        // break;
                //    }
            }

            // Set an empty rotor in each position
            foreach (KeyValuePair<EnigmaRotorPosition, List<EnigmaRotorNumber>> allowedRotors in this.m_allowedRotorsByPosition)
            {
                this.m_rotorOrder.Add(allowedRotors.Key, EnigmaRotorNumber.None);

                this.SetInitialRotorPosition(allowedRotors.Key, '\0');
            }

            BuildKey();
            BuildIV();
        }

        private void SetEnigmaType(
            EnigmaModel model,
            string keyboardLetters,
            bool hasReflector,
            EnigmaReflectorNumber relectorNumber
            )
        {
            this.m_model = model;
            this.m_keyboardLetters = new Collection<char>(keyboardLetters.ToCharArray());
            this.m_hasReflector = hasReflector;
            this.m_reflectorNumber = relectorNumber;

            // Reflector
            this.reflector = new EnigmaReflector(this.m_reflectorNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnigmaSettings_RotorAdvanced(object sender, EnigmaRotorEventArgs e)
        {
            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorNumber> rotorOrder in this.m_rotorOrder)
            {
                if (rotorOrder.Value == e.RotorNumber)
                {
                    this.AdvanceRotor(rotorOrder.Key, this.rotors[e.RotorNumber].CurrentSetting);
                    break;
                }
            }
        }

        private void EnigmaSettings_RotorAdvancedNotchHit(object sender, EnigmaRotorEventArgs e)
        {
            foreach (KeyValuePair<EnigmaRotorPosition, EnigmaRotorNumber> rotorOrder in this.m_rotorOrder)
            {
                if (e.RotorNumber == rotorOrder.Value)
                {
                    if ((int)rotorOrder.Key + 1 < this.RotorPositionCount)
                    {
                        // Move the next rotor in the rotor order on a position
                        this.rotors[this.m_rotorOrder[rotorOrder.Key + 1]].AdvanceRotor();
                    }
                    break;
                }
            }
        }

        void EnigmaSettings_RotorReversed(object sender, EnigmaRotorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void EnigmaSettings_RotorReversedNotchHit(object sender, EnigmaRotorEventArgs e)
        {
            throw new NotImplementedException();
        }

        internal void AdvanceRotorsToPosition(int positionCount)
        {
            if (positionCount < this.Counter)
            {
                ReverseRotors(this.Counter - positionCount);
            }
            else if (positionCount > this.Counter)
            {
                AdvanceRotors(positionCount - this.Counter);
            }
        }

        internal void ReverseRotors(int numberOfPositions)
        {
            // Advance the fastest rotor
            EnigmaRotor rotor = this.rotors[this.m_rotorOrder[EnigmaRotorPosition.Fastest]];

            for (int i = 0; i < numberOfPositions; i++)
            {
                rotor.AdvanceRotor();
            }
        }
        
        internal void AdvanceRotors(int numberOfPositions)
        {
            // Advance the fastest rotor
            EnigmaRotor rotor = this.rotors[this.m_rotorOrder[EnigmaRotorPosition.Fastest]];

            for (int i = 0; i < numberOfPositions; i++)
            {
                rotor.AdvanceRotor();
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        public void Dispose(bool disposing)
        {
            // A call to Dispose(false) should only clean up native resources. 
            // A call to Dispose(true) should clean up both managed and native resources.
            if (disposing)
            {
                // Dispose managed resources
                if (this.plugboard != null)
                {
                    this.plugboard.Dispose();
                }
                if (this.reflector != null)
                {
                    this.reflector.Dispose();
                }
            }
            // Free native resources
        }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
