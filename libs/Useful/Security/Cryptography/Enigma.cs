// <copyright file="Enigma.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA2000 // Dispose objects before losing scope

namespace Useful.Security.Cryptography
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography.Interfaces;

    /// <summary>
    /// Simulates the Enigma encoding machine.
    /// </summary>
    public sealed class Enigma : ClassicalSymmetricAlgorithm
    {
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
            : base("Enigma M3", settings)
        {
            KeyGenerator = new EnigmaKeyGenerator();
        }

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
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            StringBuilder output = new StringBuilder();
            foreach (char inputChar in plaintext.ToCharArray())
            {
                // Encrypt and Decrypt work the same way
                output.Append(Encrypt(inputChar));
            }

            return output.ToString();
        }

        /// <summary>
        /// Encipher a plaintext letter into an enciphered letter.  Decipher works in the same way as encipher.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The enciphered letter.</returns>
        private char Encrypt(char letter)
        {
            char newLetter;

            EnigmaSettings settings = (EnigmaSettings)Settings;

            if (letter == ' ')
            {
                return letter;
            }
            else if (!settings.CharacterSet.Contains(letter))
            {
                return '\0';
            }

            newLetter = letter;

            // Advance the rotors one position
            settings.Rotors.AdvanceRotors();

            // Plugboard
            newLetter = settings.Plugboard[newLetter];

            // Go thru the rotors forwards
            newLetter = settings.Rotors[EnigmaRotorPosition.Fastest].Forward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Second].Forward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Third].Forward(newLetter);

            // Go thru the relector
            EnigmaReflector reflector = new EnigmaReflector(settings.ReflectorNumber);
            newLetter = reflector.Reflect(newLetter);

            // Go thru the rotors backwards
            newLetter = settings.Rotors[EnigmaRotorPosition.Third].Backward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Second].Backward(newLetter);
            newLetter = settings.Rotors[EnigmaRotorPosition.Fastest].Backward(newLetter);

            newLetter = settings.Plugboard[newLetter];

            // Letter cannot encrypt to itself.
            // Debug.Assert(Letters.Clean(this.settings.AllowedLetters, letter) != Letters.Clean(this.settings.AllowedLetters, newLetter), "Letter cannot encrypt to itself.");
            return newLetter;
        }
    }
}