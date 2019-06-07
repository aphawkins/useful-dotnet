// <copyright file="Enigma.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// Simulates the Enigma encoding machine.
    /// </summary>
    public sealed class Enigma : ClassicalSymmetricAlgorithm
    {
        /////// <summary>
        /////// The size of a byte.
        /////// </summary>
        ////private const int SizeOfByte = 8;

        /////// <summary>
        /////// The length of the key.
        /////// </summary>
        ////private readonly int _lengthOfKey = 5;

        /////// <summary>
        /////// The plugboard settings.
        /////// </summary>
        ////private MonoAlphabeticCipher _plugboard;

        /////// <summary>
        /////// Has this object been disposed?.
        /////// </summary>
        ////private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Enigma"/> class.
        /// </summary>
        public Enigma()
            : this(new EnigmaSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enigma"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public Enigma(EnigmaSettings settings)
            : base("Enigma", settings)
        {
            KeyGenerator = new EnigmaKeyGenerator();

            ////// Plugboard
            ////_plugboard = new MonoAlphabeticCipher(new MonoAlphabeticSettings(((EnigmaSettings)Settings).Plugboard.Key.ToArray()));
        }

        /////// <summary>
        /////// Initializes a new instance of the <see cref="Enigma"/> class.
        /////// </summary>
        ////public Enigma() // EnigmaModel model)
        ////{
        ////    ModeValue = CipherMode.ECB;
        ////    PaddingValue = PaddingMode.None;
        ////    KeySizeValue = int.MaxValue;

        ////    // switch (model)
        ////    // {
        ////    //    case EnigmaModel.Military:
        ////    //        {
        ////    //            LengthOfKey = 5;
        ////    //            break;
        ////    //        }
        ////    //    case EnigmaModel.Navy:
        ////    //    case EnigmaModel.M4:
        ////    //        {
        ////    //            LengthOfKey = 5;
        ////    //            break;
        ////    //        }
        ////    //    default:
        ////    //        throw new Exception();
        ////    // }
        ////    BlockSizeValue = _LengthOfKey * sizeof(char) * SizeOfByte;

        ////    // FeedbackSizeValue = 2;
        ////    LegalBlockSizesValue = new KeySizes[1];
        ////    LegalBlockSizesValue[0] = new KeySizes(0, int.MaxValue, 1);
        ////    LegalKeySizesValue = new KeySizes[1];
        ////    LegalKeySizesValue[0] = new KeySizes(0, int.MaxValue, 1);

        ////    EnigmaSettings defaultSettings = EnigmaSettings.GetDefault();
        ////    KeyValue = defaultSettings.Key;
        ////    IVValue = defaultSettings.IV;

        ////    // BlockSizeValue = this.m_settings.GetIV().Length * 8;
        ////}

        /// <inheritdoc />
        public override byte[] Key
        {
            get => Settings.Key.ToArray();

            set
            {
                Settings = new EnigmaSettings(value, IV);
                base.Key = value;
            }
        }

        /// <inheritdoc />
        public override byte[] IV
        {
            get => Settings.IV.ToArray();

            set
            {
                Settings = new EnigmaSettings(Key, value);
                base.Key = value;
            }
        }

        /////// <inheritdoc/>
        ////public override byte[] Key
        ////{
        ////    get
        ////    {
        ////        return base.Key;
        ////    }

        ////    set
        ////    {
        ////        ////EnigmaSettings enigmaKey = new EnigmaSettings(value);
        ////        ////BlockSizeValue = EnigmaSettings.GetIvLength(enigmaKey.Model) * sizeof(char) * SizeOfByte;
        ////        base.Key = value;
        ////    }
        ////}

        /// <inheritdoc />
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new Enigma(new EnigmaSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Decrypt);
        }

        /// <inheritdoc />
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            ICipher cipher = new Enigma(new EnigmaSettings(rgbKey, rgbIV));
            return new ClassicalSymmetricTransform(cipher, CipherTransformMode.Encrypt);
        }

        /// <inheritdoc/>
        public override string Decrypt(string ciphertext)
        {
            return Encrypt(ciphertext);
        }

        /// <inheritdoc/>
        public override string Encrypt(string plaintext)
        {
            StringBuilder output = new StringBuilder();
            foreach (char inputChar in plaintext.ToCharArray())
            {
                ////if (Letters.IsCleanable(this.settings.AllowedLetters, inputChar))
                ////{
                    // Encrypt and Decrypt work the same way
                    output.Append(Encipher(inputChar));
                ////}
            }

            return output.ToString();
        }

        /////// <inheritdoc />
        ////public override void GenerateIV()
        ////{
        ////    IVValue = EnigmaSettings.GetRandom().IV;
        ////}

        /////// <inheritdoc />
        ////public override void GenerateKey()
        ////{
        ////    KeyValue = EnigmaSettings.GetRandom().Key;
        ////}

        /////// <inheritdoc/>
        ////protected override void Dispose(bool disposing)
        ////{
        ////    if (_disposed)
        ////    {
        ////        return;
        ////    }

        ////    // A call to Dispose(false) should only clean up native resources.
        ////    // A call to Dispose(true) should clean up both managed and native resources.
        ////    if (disposing)
        ////    {
        ////        // Dispose managed resources
        ////        if (_plugboard != null)
        ////        {
        ////            _plugboard.Dispose();
        ////        }

        ////        if (_reflector != null)
        ////        {
        ////            _reflector.Dispose();
        ////        }
        ////    }

        ////    // Free native resources
        ////    _disposed = true;

        ////    // Call base class implementation.
        ////    base.Dispose(disposing);
        ////}

        /// <summary>
        /// Encipher a plaintext letter into an enciphered letter.  Decipher works in the same way as encipher.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The enciphered letter.</returns>
        private char Encipher(char letter)
        {
            char newLetter;

            EnigmaSettings settings = (EnigmaSettings)Settings;

            if (letter == ' ')
            {
                return letter;
            }
            else if (!EnigmaSettings.CharacterSet.Contains(letter))
            {
                return '\0';
            }

            newLetter = letter;

            // Ensure all the rotors are set correctly
            ////this.AdvanceRotorsToPosition(this.settings.Counter);

            // Advance the rotors one position
            AdvanceRotors(1);

            // Plugboard
            newLetter = settings.Plugboard[newLetter];

            // Go thru the rotors forwards
            newLetter = settings.Rotors[EnigmaRotorPosition.Fastest].Forward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Second].Forward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Third].Forward(newLetter);

            using (EnigmaReflector reflector = new EnigmaReflector(settings.ReflectorNumber))
            {
                // Go thru the relector
                newLetter = reflector.Reflect(newLetter);
            }

            // Go thru the rotors backwards
            newLetter = settings.Rotors[EnigmaRotorPosition.Third].Backward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Second].Backward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Fastest].Backward(newLetter);

            newLetter = settings.Plugboard.Reverse(newLetter);

            // Letter cannot encrypt to itself.
            // Debug.Assert(Letters.Clean(this.settings.AllowedLetters, letter) != Letters.Clean(this.settings.AllowedLetters, newLetter), "Letter cannot encrypt to itself.");
            return newLetter;
        }

        /// <summary>
        /// Advances the rotors by a specified number of positions.
        /// </summary>
        /// <param name="numberOfPositions">The number of positions to move the rotors.</param>
        private void AdvanceRotors(int numberOfPositions)
        {
            // Advance the fastest rotor
            EnigmaRotor rotor = ((EnigmaSettings)Settings).Rotors[EnigmaRotorPosition.Fastest];

            for (int i = 0; i < numberOfPositions; i++)
            {
                rotor.AdvanceRotor();
            }
        }
    }
}