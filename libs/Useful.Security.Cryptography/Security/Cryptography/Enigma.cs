// <copyright file="Enigma.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Text;

    /// <summary>
    /// Simulates the Enigma encoding machine.
    /// </summary>
    public sealed class Enigma : ICipher
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="Enigma"/> class.
        /// </summary>
        /// <param name="settings">The cipher's settings.</param>
        public Enigma(IEnigmaSettings settings) => Settings = settings;

        /// <inheritdoc />
        public string CipherName => "Enigma M3";

        /// <summary>
        /// Gets or sets settings.
        /// </summary>
        public IEnigmaSettings Settings { get; set; }

        /// <inheritdoc />
        public override string ToString() => CipherName;

        /// <inheritdoc />
        public string Decrypt(string ciphertext) => Encrypt(ciphertext);

        /// <inheritdoc />
        public string Encrypt(string plaintext)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException(nameof(plaintext));
            }

            StringBuilder output = new();
            foreach (char inputChar in plaintext.ToCharArray())
            {
                // Encrypt and Decrypt work the same way
                char cipherChar = Encrypt(inputChar);
                if (cipherChar != '\0')
                {
                    output.Append(cipherChar);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Encrypt a plaintext letter into an enciphered letter.  Decipher works in the same way as encipher.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The encrypted letter.</returns>
        private char Encrypt(char letter)
        {
            char newLetter;

            if (letter == ' ')
            {
                return letter;
            }
            else if (!CharacterSet.Contains(letter))
            {
                return '\0';
            }

            newLetter = letter;

            // Advance the rotors one position
            Settings.Rotors.AdvanceRotors();

            // Plugboard
            newLetter = Settings.Plugboard[newLetter];

            // Go thru the rotors forwards
            newLetter = Settings.Rotors[EnigmaRotorPosition.Fastest].Forward(newLetter);
            newLetter = Settings.Rotors[EnigmaRotorPosition.Second].Forward(newLetter);
            newLetter = Settings.Rotors[EnigmaRotorPosition.Third].Forward(newLetter);

            // Go thru the relector
            newLetter = Settings.Reflector.Reflect(newLetter);

            // Go thru the rotors backwards
            newLetter = Settings.Rotors[EnigmaRotorPosition.Third].Backward(newLetter);
            newLetter = Settings.Rotors[EnigmaRotorPosition.Second].Backward(newLetter);
            newLetter = Settings.Rotors[EnigmaRotorPosition.Fastest].Backward(newLetter);

            newLetter = Settings.Plugboard[newLetter];

            // Letter cannot encrypt to itself.
            // Debug.Assert(Letters.Clean(this.settings.AllowedLetters, letter) != Letters.Clean(this.settings.AllowedLetters, newLetter), "Letter cannot encrypt to itself.");
            return newLetter;
        }
    }
}