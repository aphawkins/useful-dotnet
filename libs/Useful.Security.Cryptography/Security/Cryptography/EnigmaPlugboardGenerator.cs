// <copyright file="EnigmaPlugboardGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    /// <summary>
    /// Enigma Plugboard settings generator.
    /// </summary>
    internal static class EnigmaPlugboardGenerator
    {
        public static IEnigmaPlugboard Generate()
        {
            IReflectorSettings reflector = ReflectorSettingsGenerator.Generate();

            IList<EnigmaPlugboardPair> pairs = new List<EnigmaPlugboardPair>();
            IList<char> usedLetters = new List<char>();

            for (int i = 0; i < reflector.CharacterSet.Count; i++)
            {
                char fromLetter = reflector.CharacterSet[i];
                char toLetter = reflector.Substitutions[i];

                if (!usedLetters.Contains(fromLetter)
                    && !usedLetters.Contains(toLetter))
                {
                    usedLetters.Add(fromLetter);
                    usedLetters.Add(toLetter);
                    pairs.Add(new EnigmaPlugboardPair() { From = fromLetter, To = toLetter });
                }
            }

            return new EnigmaPlugboard(pairs);
        }
    }
}