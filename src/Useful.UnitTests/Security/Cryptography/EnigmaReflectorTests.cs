// <copyright file="EnigmaReflectorTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaReflectorTests
    {
        public static TheoryData<EnigmaReflectorNumber, string> Data => new TheoryData<EnigmaReflectorNumber, string>
        {
            { EnigmaReflectorNumber.A, "EJMZALYXVBWFCRQUONTSPIKHGD" },
            { EnigmaReflectorNumber.B, "YRUHQSLDPXNGOKMIEBFZCWVJAT" },
            { EnigmaReflectorNumber.C, "FVPJIAOYEDRZXWGCTKUQSBNMHL" },
            { EnigmaReflectorNumber.BThin, "ENKQAUYWJICOPBLMDXZVFTHRGS" },
            { EnigmaReflectorNumber.CThin, "RDOBJNTKVEHMLFCWZAXGYIPSUQ" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Reflect(EnigmaReflectorNumber reflectorNumber, string reflection)
        {
            string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            using (EnigmaReflector reflector = new EnigmaReflector(reflectorNumber))
            {
                Assert.Equal(reflectorNumber, reflector.ReflectorNumber);

                for (int i = 0; i < characterSet.Length; i++)
                {
                    Assert.Equal(reflection[i], reflector.Reflect(characterSet[i]));
                }
            }
        }
    }
}