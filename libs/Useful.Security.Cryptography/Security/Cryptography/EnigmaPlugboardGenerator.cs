// <copyright file="EnigmaPlugboardGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;

    /// <summary>
    /// Enigma Plugboard settings generator.
    /// </summary>
    internal sealed class EnigmaPlugboardGenerator
    {
        public static IEnigmaPlugboard Generate()
        {
            IReflectorSettings reflector = ReflectorSettingsGenerator.Generate();

            IDictionary<char, char> pairs = new Dictionary<char, char>();
            IList<char> usedLetters = new List<char>();

            for (int i = 0; i < reflector.CharacterSet.Length; i++)
            {
                char fromLetter = reflector.CharacterSet[i];
                char toLetter = reflector.Substitutions[i];

                if (!usedLetters.Contains(fromLetter)
                    && !usedLetters.Contains(toLetter))
                {
                    usedLetters.Add(fromLetter);
                    usedLetters.Add(toLetter);
                    pairs.Add(fromLetter, toLetter);
                }
            }

            return new EnigmaPlugboard(pairs);
        }
    }
}