// <copyright file="EnigmaReflectorTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

using Useful.Security.Cryptography;
using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    public class EnigmaReflectorTests
    {
        [Fact]
        public void Defaults()
        {
            IEnigmaReflector reflector = new EnigmaReflector();
            Assert.Equal(EnigmaReflectorNumber.B, reflector.ReflectorNumber);
        }

        [Theory]
        [InlineData(EnigmaReflectorNumber.B, "YRUHQSLDPXNGOKMIEBFZCWVJAT")]
        [InlineData(EnigmaReflectorNumber.C, "FVPJIAOYEDRZXWGCTKUQSBNMHL")]
        public void Reflect(EnigmaReflectorNumber reflectorNumber, string wiring)
        {
            string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            IEnigmaReflector reflector = new EnigmaReflector()
            {
                ReflectorNumber = reflectorNumber,
            };
            Assert.Equal(reflectorNumber, reflector.ReflectorNumber);

            for (int i = 0; i < characterSet.Length; i++)
            {
                Assert.Equal(wiring[i], reflector.Reflect(characterSet[i]));
            }
        }
    }
}