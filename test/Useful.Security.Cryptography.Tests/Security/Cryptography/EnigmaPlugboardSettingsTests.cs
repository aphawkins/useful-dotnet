// <copyright file="EnigmaPlugboardSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaPlugboardSettingsTests
    {
        public static TheoryData<IDictionary<char, char>> InvalidPairs => new TheoryData<IDictionary<char, char>>
        {
            { new Dictionary<char, char>() { { 'A', 'B' }, { 'B', 'A' } } }, // Repeat letters
            { new Dictionary<char, char>() { { 'a', 'B' } } }, // Subs incorrect case
            { new Dictionary<char, char>() { { 'A', 'A' } } }, // Same letter
        };

        public static TheoryData<IDictionary<char, char>, int> ValidPairs => new TheoryData<IDictionary<char, char>, int>
        {
            { new Dictionary<char, char>() { { 'A', 'B' }, { 'C', 'D' } }, 2 },
        };

        [Fact]
        public void CtorEmpty()
        {
            IEnigmaPlugboardSettings settings = new EnigmaPlugboardSettings();
            Assert.Equal(0, settings.SubstitutionCount);
            Assert.Empty(settings.Substitutions());
            Assert.Equal('A', settings['A']);
        }

        [Theory]
        [MemberData(nameof(InvalidPairs))]
        public void CtorSubstitutionsInvalid(IDictionary<char, char> pairs) => Assert.Throws<ArgumentException>(nameof(pairs), () => new EnigmaPlugboardSettings(pairs));

        [Fact]
        public void CtorSubstitutionsNull() => Assert.Throws<ArgumentNullException>("pairs", () => new EnigmaPlugboardSettings(null));

        [Theory]
        [MemberData(nameof(ValidPairs))]
        public void CtorSubstitutionsValid(IDictionary<char, char> pairs, int substitutionCount)
        {
            IEnigmaPlugboardSettings settings = new EnigmaPlugboardSettings(pairs);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
            Assert.Equal(pairs.Values.ToArray()[0], settings[pairs.Keys.ToArray()[0]]);
            Assert.Equal(pairs.Keys.ToArray()[0], settings[pairs.Values.ToArray()[0]]);
        }
    }
}