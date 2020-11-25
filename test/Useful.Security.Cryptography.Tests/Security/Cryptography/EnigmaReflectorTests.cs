// <copyright file="EnigmaReflectorTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaReflectorTests
    {
        [Theory]
        [InlineData(EnigmaReflectorNumber.B, "YRUHQSLDPXNGOKMIEBFZCWVJAT")]
        [InlineData(EnigmaReflectorNumber.C, "FVPJIAOYEDRZXWGCTKUQSBNMHL")]
        public void Reflect(EnigmaReflectorNumber reflectorNumber, string reflection)
        {
            string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            EnigmaReflector reflector = new(reflectorNumber);
            Assert.Equal(reflectorNumber, reflector.ReflectorNumber);

            for (int i = 0; i < characterSet.Length; i++)
            {
                Assert.Equal(reflection[i], reflector.Reflect(characterSet[i]));
            }
        }
    }
}