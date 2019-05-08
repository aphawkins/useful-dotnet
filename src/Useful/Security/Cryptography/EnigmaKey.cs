//-----------------------------------------------------------------------
// <copyright file="EnigmaKey.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>A structure for the Enigma's key.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System;

    /// <summary>
    /// A structure for the Enigma's key.
    /// </summary>
    internal class EnigmaKey
    {
        #region Properties
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
        #endregion

        #region ctor
        /// <summary>
        /// Initializes a new instance of the MonoAlphabeticSettings class.
        /// </summary>
        /// <param name="key">The encryption Key.</param>
        /// <param name="iv">The Initialization Vector.</param>
        public EnigmaKey()
        {
            this.RotorSettings = EnigmaRotorOrder.Create(this.Model);
            this.PlugboardPairs = new Collection<SubstitutionPair>();
        }
        #endregion

        #region Methods
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.RotorSettings != null);
            Contract.Invariant(this.PlugboardPairs != null);
            Contract.Invariant(Enum.IsDefined(typeof(EnigmaModel), this.Model));
            //Contract.Invariant(Contract.ForAll<KeyValuePair<EnigmaRotorPosition, EnigmaRotorSettings>>(this.RotorSettings, x => Enum.IsDefined(typeof(EnigmaRotorPosition), x.Key)));
            //Contract.Invariant(Contract.ForAll<KeyValuePair<EnigmaRotorPosition, EnigmaRotorSettings>>(this.RotorSettings, x => Enum.IsDefined(typeof(EnigmaRotorNumber), x.Value)));
            Contract.Invariant(Contract.ForAll<SubstitutionPair>(this.PlugboardPairs, x => x != null));
        }

        #endregion
    }
}
