//-----------------------------------------------------------------------
// <copyright file="EnigmaUINameConverter.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>Names for Enigma elements.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System.Globalization;
    using System.Security.Cryptography;
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Names for Enigma elements.
    /// </summary>
    public static class EnigmaUINameConverter
    {
        #region EnigmaRotorNumber
        /// <summary>
        /// Gets an rotor from a name.
        /// </summary>
        /// <param name="textRepresentation">The name of the rotor.</param>
        /// <returns>The rotor.</returns>
        public static EnigmaRotorNumber Convert(string textRepresentation)
        {
            Contract.Requires(textRepresentation != null);
            Contract.Requires(textRepresentation.Length > 0);
            Contract.Requires(!textRepresentation.Contains("\0"));

            if (string.Compare(textRepresentation, Useful.Resource.None, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.None;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_1, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.One;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_2, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Two;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_3, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Three;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_4, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Four;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_5, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Five;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_6, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Six;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_7, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Seven;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.RomanNumeral_8, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Eight;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.Greek_Beta, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Beta;
            }
            else if (string.Compare(textRepresentation, Useful.Resource.Greek_Gamma, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return EnigmaRotorNumber.Gamma;
            }
            else
            {
                throw new CryptographicException("Unknown rotor number.");
            }
        }

        /// <summary>
        /// Gets a name for a rotor.
        /// </summary>
        /// <param name="rotorNumber">The rotor to get the name of.</param>
        /// <returns>The name of the rotor.</returns>
        public static string Convert(EnigmaRotorNumber rotorNumber)
        {
            Contract.Requires(Enum.IsDefined(typeof(EnigmaRotorNumber), rotorNumber));

            switch (rotorNumber)
            {
                case EnigmaRotorNumber.None:
                    {
                        return Resource.None;
                    }

                case EnigmaRotorNumber.One:
                    {
                        return Resource.RomanNumeral_1;
                    }

                case EnigmaRotorNumber.Two:
                    {
                        return Resource.RomanNumeral_2;
                    }

                case EnigmaRotorNumber.Three:
                    {
                        return Resource.RomanNumeral_3;
                    }

                case EnigmaRotorNumber.Four:
                    {
                        return Resource.RomanNumeral_4;
                    }

                case EnigmaRotorNumber.Five:
                    {
                        return Resource.RomanNumeral_5;
                    }

                case EnigmaRotorNumber.Six:
                    {
                        return Resource.RomanNumeral_6;
                    }

                case EnigmaRotorNumber.Seven:
                    {
                        return Resource.RomanNumeral_7;
                    }

                case EnigmaRotorNumber.Eight:
                    {
                        return Resource.RomanNumeral_8;
                    }

                case EnigmaRotorNumber.Beta:
                    {
                        return Resource.Greek_Beta;
                    }

                case EnigmaRotorNumber.Gamma:
                    {
                        return Resource.Greek_Gamma;
                    }

                default:
                    {
                        throw new CryptographicException("Unknown rotor number.");
                    }
            }
        }
        #endregion

        #region EnigmaRotorPosition
        /// <summary>
        /// Gets a name for a rotor position.
        /// </summary>
        /// <param name="rotorPosition">The rotor position to get the name of.</param>
        /// <returns>The name of the rotor position.</returns>
        public static string Convert(EnigmaRotorPosition rotorPosition)
        {
            // TODO: strings from resources
            switch (rotorPosition)
            {
                case EnigmaRotorPosition.Fastest:
                    {
                        return "Fastest";
                    }

                case EnigmaRotorPosition.Second:
                    {
                        return "Second";
                    }

                case EnigmaRotorPosition.Third:
                    {
                        return "Third";
                    }

                case EnigmaRotorPosition.Forth:
                    {
                        return "Forth";
                    }

                default:
                    {
                        throw new CryptographicException("Unknown rotor position.");
                    }
            }
        }
        #endregion
    }
}
