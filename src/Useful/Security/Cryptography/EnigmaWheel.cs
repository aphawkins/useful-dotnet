namespace Useful.Security.Cryptography
{
    /// <summary>
    /// An Enigma wheel (Wolze).
    /// </summary>
    public struct EnigmaWheel
    {
        private EnigmaScrambler _myScrambler;

        /// <summary>
        /// Gets or sets the letter the wheel currently has.
        /// </summary>
        public char CurrentLetter { get; set; }

        /// <summary>
        /// Gets or sets the scrambler used for this wheel.
        /// </summary>
        public EnigmaScrambler MyScrambler
        {
            get
            {
                return _myScrambler;
            }

            set
            {
                _myScrambler = value;
            }
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the objects are equal, else false.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Retrieves a value that indicates the hash code value for this object.
        /// </summary>
        /// <returns>The hash code value for this object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating whether this object's value is equal to another.
        /// </summary>
        /// <param name="operandX">This object.</param>
        /// <param name="operandY">The object to compare.</param>
        /// <returns>true if the objects are equal; otherwise false.</returns>
        public static bool operator ==(EnigmaWheel operandX, EnigmaWheel operandY)
        {
            return operandX == operandY;
        }

        /// <summary>
        /// Returns a value indicating whether this object's value is not equal to another.
        /// </summary>
        /// <param name="operandX">This object.</param>
        /// <param name="operandY">The object to compare.</param>
        /// <returns>true if the objects are not equal; otherwise false.</returns>
        public static bool operator !=(EnigmaWheel operandX, EnigmaWheel operandY)
        {
            return operandX != operandY;
        }
    }
}