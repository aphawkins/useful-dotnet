﻿namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Holds all the ciphers.
    /// </summary>
    public class CipherRepository : IRepository<ICipher>
    {
        private List<ICipher> ciphers = new List<ICipher>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherRepository"/> class.
        /// It will be empty.
        /// </summary>
        public CipherRepository()
        {
        }

        /// <summary>
        /// Gets the current cipher.
        /// </summary>
        public ICipher CurrentItem
        {
            get;
            private set;
        }

        /// <summary>
        /// Loads all the ciphers into a new repository.
        /// </summary>
        /// <returns>A new instance of the class containing all the ciphers.</returns>
        public static CipherRepository Create()
        {
            CipherRepository repository = new CipherRepository();
            repository.ciphers = new List<ICipher>
            {
                new ReverseCipher(),
                new ROT13Cipher()
            };

            repository.CurrentItem = repository.ciphers[0];
            return repository;
        }

        /// <summary>
        /// Adds a new cipher to the repository.
        /// </summary>
        /// <param name="cipher">The new cipher to add.</param>
        public void Create(ICipher cipher)
        {
            this.ciphers.Add(cipher);
        }

        /// <summary>
        /// Removes a cipher from the repository.
        /// </summary>
        /// <param name="cipher">The cipher to delete.</param>
        public void Delete(ICipher cipher)
        {
            int removeAt = -1;

            for (int i = 0; i < this.ciphers.Count; i++)
            {
                if (this.ciphers[i].CipherName == cipher.CipherName)
                {
                    removeAt = i;
                    break;
                }
            }

            if (removeAt > -1)
            {
                this.ciphers.RemoveAt(removeAt);
            }
        }

        /// <summary>
        /// Retrieves all the ciphers.
        /// </summary>
        /// <returns>All the ciphers.</returns>
        public List<ICipher> Read()
        {
            return this.ciphers;
        }

        /// <summary>
        /// Sets the <see cref="CurrentItem" /> according to the match criteria.
        /// </summary>
        /// <param name="match">The criteria to find the current cipher.</param>
        public void SetCurrentItem(Func<ICipher, bool> match)
        {
            if (this.ciphers.Count == 0)
            {
                return;
            }

            this.CurrentItem = this.ciphers.First(match);
        }

        /// <summary>
        /// Updates a cipher in the repository.
        /// </summary>
        /// <param name="cipher">The cipher to update.</param>
        public void Update(ICipher cipher)
        {
            for (int i = 0; i < this.ciphers.Count; i++)
            {
                if (this.ciphers[i].CipherName == cipher.CipherName)
                {
                    this.ciphers[i] = cipher;
                }
            }
        }
    }
}