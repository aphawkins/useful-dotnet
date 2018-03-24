// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Useful
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A generic repository.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the repository.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets the current item.
        /// </summary>
        T CurrentItem
        {
            get;
        }

        /// <summary>
        /// Sets the <see cref="CurrentItem" /> according to the match criteria.
        /// </summary>
        /// <param name="match">The criteria to find the current item.</param>
        void SetCurrentItem(Func<T, bool> match);

        /// <summary>
        /// Adds a new item to the repository.
        /// </summary>
        /// <param name="item">The new item to add.</param>
        void Create(T item);

        /// <summary>
        /// Retrieves all the items.
        /// </summary>
        /// <returns>All the items.</returns>
        IList<T> Read();

        /// <summary>
        /// Updates an item in the repository.
        /// </summary>
        /// <param name="item">The item to update.</param>
        void Update(T item);

        /// <summary>
        /// Removes an item from the repository.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        void Delete(T item);
    }
}