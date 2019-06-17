// <copyright file="EnigmaRotorNumberSorter.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A comparer for sorting Enigma rotors.
    /// </summary>
    public class EnigmaRotorNumberSorter : IComparer<EnigmaRotorNumber>
    {
        /// <summary>
        /// Case insensitive comparer object.
        /// </summary>
        private CaseInsensitiveComparer _objectCompare;

        /// <summary>
        /// Initializes a new instance of the EnigmaRotorNumberSorter class.
        /// </summary>
        /// <param name="order">The order of sorting to apply (for example, 'Ascending' or 'Descending').</param>
        public EnigmaRotorNumberSorter(SortOrder order)
        {
            // Initialize the sort order to 'none'
            Order = order;

            // Initialize the CaseInsensitiveComparer object
            _objectCompare = new CaseInsensitiveComparer(Culture.CurrentCulture);
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order { get; set; }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared.</param>
        /// <param name="y">Second object to be compared.</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'.</returns>
        public int Compare(EnigmaRotorNumber x, EnigmaRotorNumber y)
        {
            int compareResult;

            // Compare the two items
            // Change this line if you want to sort in a different manner (such as numerically).
            compareResult = _objectCompare.Compare((int)x, (int)y);

            // Calculate correct return value based on object comparison
            if (Order == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (Order == SortOrder.Descending)
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
    }
}