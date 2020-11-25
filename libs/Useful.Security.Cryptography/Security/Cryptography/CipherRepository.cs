// <copyright file="CipherRepository.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Holds all the ciphers.
    /// </summary>
    public class CipherRepository : IRepository<ICipher>
    {
        private readonly List<ICipher> _ciphers = new();

        /// <summary>
        /// Gets or sets the current cipher.
        /// </summary>
        public ICipher? CurrentItem
        {
            get;
            protected set;
        }

        /// <summary>
        /// Adds a new cipher to the repository.
        /// </summary>
        /// <param name="cipher">The new cipher to add.</param>
        public void Create(ICipher cipher) => _ciphers.Add(cipher);

        /// <summary>
        /// Removes a cipher from the repository.
        /// </summary>
        /// <param name="cipher">The cipher to delete.</param>
        public void Delete(ICipher cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException(nameof(cipher));
            }

            int removeAt = -1;

            for (int i = 0; i < _ciphers.Count; i++)
            {
                if (_ciphers[i].CipherName == cipher.CipherName)
                {
                    removeAt = i;
                    break;
                }
            }

            if (removeAt > -1)
            {
                _ciphers.RemoveAt(removeAt);
            }
        }

        /// <summary>
        /// Retrieves all the ciphers.
        /// </summary>
        /// <returns>All the ciphers.</returns>
        public IEnumerable<ICipher> Read() => _ciphers;

        /// <summary>
        /// Sets the <see cref="CurrentItem" /> according to the match criteria.
        /// </summary>
        /// <param name="match">The criteria to find the current cipher.</param>
        public void SetCurrentItem(Func<ICipher, bool> match)
        {
            if (_ciphers.Count == 0)
            {
                return;
            }

            CurrentItem = _ciphers.First(match);
        }

        /// <summary>
        /// Updates a cipher in the repository.
        /// </summary>
        /// <param name="cipher">The cipher to update.</param>
        public void Update(ICipher cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException(nameof(cipher));
            }

            for (int i = 0; i < _ciphers.Count; i++)
            {
                if (_ciphers[i].CipherName == cipher.CipherName)
                {
                    _ciphers[i] = cipher;
                }
            }
        }
    }
}