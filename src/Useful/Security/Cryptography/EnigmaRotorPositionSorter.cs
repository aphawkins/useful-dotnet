//-----------------------------------------------------------------------
// <copyright file="EnigmaRotorPositionSorter.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>A comparer for sorting Enigma rotor positions.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A comparer for sorting Enigma rotor positions.
    /// </summary>
    public class EnigmaRotorPositionSorter : IComparer<EnigmaRotorPosition>
    {
        #region Fields
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer objectCompare;
        #endregion

        #region ctor
        /// <summary>
        /// Initializes a new instance of the EnigmaRotorPositionSorter class.
        /// </summary>
        /// <param name="orderOfSort"> Specifies the order of sorting to apply (for example, 'Ascending' or 'Descending').</param>
        public EnigmaRotorPositionSorter(SortOrder orderOfSort)
        {
            // Initialize the sort order to 'none'
            this.Order = orderOfSort;

            // Initialize the CaseInsensitiveComparer object
            this.objectCompare = new CaseInsensitiveComparer(Culture.CurrentCulture);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(EnigmaRotorPosition x, EnigmaRotorPosition y)
        {
            int compareResult;

            // Compare the two items
            // Change this line if you want to sort in a different manner (such as numerically).
            compareResult = this.objectCompare.Compare((int)x, (int)y);

            // Calculate correct return value based on object comparison
            if (this.Order == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (this.Order == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return -compareResult;
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }
        #endregion
    }
}
