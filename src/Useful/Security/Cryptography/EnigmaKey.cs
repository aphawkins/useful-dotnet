// <copyright file="EnigmaKey.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// A structure for the Enigma's key.
    /// </summary>
    internal class EnigmaKey
    {
        /// <summary>
        /// Gets or sets the Enigma model type.
        /// </summary>
        internal EnigmaModel Model { get; set; }

        /// <summary>
        /// Gets or sets the Enigma reflector type.
        /// </summary>
        internal EnigmaReflectorNumber Reflector { get; set; }

        /// <summary>
        /// Gets or sets the position each rotor is in.
        /// </summary>
        internal EnigmaRotorOrder RotorSettings { get; set; }

        /// <summary>
        /// Gets or sets a swapped plugboard pair.
        /// </summary>
        internal Collection<SubstitutionPair> PlugboardPairs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaKey"/> class.
        /// </summary>
        public EnigmaKey()
        {
            RotorSettings = EnigmaRotorOrder.Create(Model);
            PlugboardPairs = new Collection<SubstitutionPair>();
        }
    }
}