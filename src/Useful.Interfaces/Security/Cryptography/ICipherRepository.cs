// <copyright file="ICipherRepository.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Interfaces
{
    using Useful.Interfaces;

    /// <summary>
    /// A generic repository.
    /// </summary>
    public interface ICipherRepository : IRepository<ICipher>
    {
    }
}