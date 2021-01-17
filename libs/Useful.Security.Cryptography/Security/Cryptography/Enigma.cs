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
                if (inputChar == ' ')
                {
                    output.Append(' ');
                }
                else if (CharacterSet.Contains(char.ToUpper(inputChar)))
                {
                    output.Append(Encrypt(char.ToUpper(inputChar)));
                }
            }

            return output.ToString();
        }

        /// <inheritdoc />
        public void GenerateSettings() => Settings = EnigmaSettingsGenerator.Generate();

        /// <summary>
        /// Encrypt a plaintext letter into an enciphered letter.  Decipher works in the same way as encipher.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The encrypted letter.</returns>
        private char Encrypt(char letter)
        {
            // Advance the rotors one position
            Settings.Rotors.AdvanceRotors();

            // Plugboard
            letter = Settings.Plugboard[letter];

            // Go thru the rotors forwards
            letter = Settings.Rotors[EnigmaRotorPosition.Fastest].Forward(letter);
            letter = Settings.Rotors[EnigmaRotorPosition.Second].Forward(letter);
            letter = Settings.Rotors[EnigmaRotorPosition.Third].Forward(letter);

            // Go thru the relector
            letter = Settings.Reflector.Reflect(letter);

            // Go thru the rotors backwards
            letter = Settings.Rotors[EnigmaRotorPosition.Third].Backward(letter);
            letter = Settings.Rotors[EnigmaRotorPosition.Second].Backward(letter);
            letter = Settings.Rotors[EnigmaRotorPosition.Fastest].Backward(letter);

            letter = Settings.Plugboard[letter];

            // Letter cannot encrypt to itself.
            // Debug.Assert(Letters.Clean(this.settings.AllowedLetters, letter) != Letters.Clean(this.settings.AllowedLetters, newLetter), "Letter cannot encrypt to itself.");
            return letter;
        }
    }
}