// <copyright file="MonoAlphabeticKeyGenerator.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Interfaces.Security.Cryptography;

    /// <summary>
    /// MonoAlphabetic key generator.
    /// </summary>
    public class MonoAlphabeticKeyGenerator : IKeyGenerator
    {
        /// <inheritdoc />
        public byte[] RandomIv() => Array.Empty<byte>();

        /// <inheritdoc />
        public byte[] RandomKey()
        {
            byte[] randomNumber = new byte[1];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomNumber);
                return Encoding.Unicode.GetBytes($"{randomNumber[0] % 26}");
            }
        }

        /////// <summary>
        /////// Gets some random substitutions.
        /////// </summary>
        /////// <param name="allowedLetters">The allowed letters to the used in the substitutions.</param>
        /////// <param name="isSymmetric">If the substitutions are symmetric.</param>
        /////// <returns>A random collection of substitutions.</returns>
        ////private static IEnumerable<byte> GetRandomKey(IEnumerable<char> allowedLetters, bool isSymmetric)
        ////{
        ////    List<char> allowedLettersCloneFrom = new List<char>(allowedLetters);
        ////    List<char> allowedLettersCloneTo = new List<char>(allowedLetters);

        ////    List<byte> key = (List<byte>)BuildKey(allowedLetters, allowedLetters, isSymmetric);

        ////    MonoAlphabeticSettings mono = Create(key);

        ////    Random rnd = new Random();
        ////    int indexFrom;
        ////    int indexTo;

        ////    char from;
        ////    char to;

        ////    while (allowedLettersCloneFrom.Count > 0)
        ////    {
        ////        indexFrom = rnd.Next(0, allowedLettersCloneFrom.Count);

        ////        //// Extensions.IndexOutOfRange(indexFrom, 0, allowedLettersCloneFrom.Count - 1);

        ////        from = allowedLettersCloneFrom[indexFrom];
        ////        allowedLettersCloneFrom.RemoveAt(indexFrom);
        ////        if (isSymmetric
        ////            && allowedLettersCloneTo.Contains(from))
        ////        {
        ////            allowedLettersCloneTo.Remove(from);
        ////        }

        ////        indexTo = rnd.Next(0, allowedLettersCloneTo.Count);
        ////        to = allowedLettersCloneTo[indexTo];

        ////        allowedLettersCloneTo.RemoveAt(indexTo);
        ////        if (isSymmetric
        ////            && allowedLettersCloneFrom.Contains(to))
        ////        {
        ////            allowedLettersCloneFrom.Remove(to);
        ////        }

        ////        ////if (from == to)
        ////        ////{
        ////        ////    continue;
        ////        ////}

        ////        mono[from] = to;
        ////    }

        ////    return mono.Key;
        ////}
    }
}