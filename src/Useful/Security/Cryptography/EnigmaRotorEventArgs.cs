//-----------------------------------------------------------------------
// <copyright file="EnigmaRotorEventArgs.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>The arguments for an EnigmaRotorEvent.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System;

    /// <summary>
    /// The arguments for an EnigmaRotorEvent.
    /// </summary>
    [Serializable]
    public class EnigmaRotorAdvanceEventArgs : EventArgs
    {
        #region ctor
        /// <summary>
        /// Initializes a new instance of the EnigmaRotorEventArgs class.
        /// </summary>
        /// <param name="rotorNumber">The rotor number that this notch belongs to.</param>
        /// <param name="isNotchHit">Flag to say if a notch has been hit.</param>
        /// <param name="isDoubleStep">Flag to say if a DoubleStep has been hit (one after a notch) has been hit.</param>
        public EnigmaRotorAdvanceEventArgs(EnigmaRotorNumber rotorNumber, bool isNotchHit, bool isDoubleStep)
        {
            this.RotorNumber = rotorNumber;
            this.IsNotchHit = isNotchHit;
            this.IsDoubleStep = isDoubleStep;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Gets the message to send.
        /// </summary>
        public EnigmaRotorNumber RotorNumber { get; private set; }

        /// <summary>
        /// Gets the flag to say if a notch has been hit.
        /// </summary>
        public bool IsNotchHit { get; private set; }

        /// <summary>
        /// Gets the flag to say if a DoubleStep has been hit (one after a notch) has been hit.
        /// </summary>
        public bool IsDoubleStep { get; private set; }
        #endregion
    }
}
