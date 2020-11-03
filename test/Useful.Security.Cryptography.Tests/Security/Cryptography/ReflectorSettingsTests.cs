// <copyright file="ReflectorSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using Useful.Security.Cryptography;
    using Xunit;

    public class ReflectorSettingsTests
    {
        [Fact]
        public void CtorEmpty()
        {
            IReflectorSettings settings = new ReflectorSettings();
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", settings.CharacterSet);
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", settings.Substitutions);
            Assert.Equal(0, settings.SubstitutionCount);
        }

        [Theory]
        [InlineData("", "ABC")] // No character set
        [InlineData(" ABCD", "ABCD")] // Character set spacing
        [InlineData("AB CD", "ABCD")] // Character set spacing
        [InlineData("ABCD ", "ABCD")] // Character set spacing
        public void CtorCharacterSetInvalid(string characterSet, string substitutions) => Assert.Throws<ArgumentException>(nameof(characterSet), () => new ReflectorSettings(characterSet, substitutions));

        [Theory]
        [InlineData("ABC", "ABCD")] // Too many subs
        [InlineData("ABCD", " ABCD")] // Subs spacing
        [InlineData("ABCD", "ABCD ")] // Subs spacing
        [InlineData("ABCD", "AB  CD")] // Subs spacing
        [InlineData("ABCD", "aBCD")] // Subs incorrect case
        [InlineData("ABCD", "AAAA")] // Incorrect subs letters
        [InlineData("ABC", "BCA")] // Subs non-reflective
        public void CtorSubstitutionsInvalid(string characterSet, string substitutions) => Assert.Throws<ArgumentException>(nameof(substitutions), () => new ReflectorSettings(characterSet, substitutions));

        [Theory]
        [InlineData("ABC", "ABC", 0)]
        [InlineData("ABC", "BAC", 2)]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "BADCEFGHIJKLMNOPQRSTUVWXYZ", 4)]
        [InlineData("ØA", "ØA", 0)]
        public void CtorSettings(string characterSet, string substitutions, int substitutionCount)
        {
            IReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);
            Assert.Equal(characterSet, settings.CharacterSet);
            Assert.Equal(substitutions, settings.Substitutions);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
        }

        [Theory]
        [InlineData('Ø', 'A')]
        public void GetSubstitutionsInvalid(char from, char to)
        {
            IReflectorSettings settings = new ReflectorSettings();

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
        }

        [Theory]
        [InlineData('A', 'A', 0)]
        public void GetSubstitutionsValid(char from, char to, int substitutionCount)
        {
            IReflectorSettings settings = new ReflectorSettings();
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
        }

        [Fact]
        public void Reverse()
        {
            IReflectorSettings settings = new ReflectorSettings();
            Assert.Equal('A', settings.Reflect('A'));
            Assert.Equal('B', settings.Reflect('B'));
            settings['A'] = 'B';
            Assert.Equal('A', settings.Reflect('B'));
            Assert.Equal('B', settings.Reflect('A'));
        }

        [Theory]
        [InlineData('Ø')] // Invalid
        [InlineData('a')] // Incorrect case
        [InlineData(' ')] // Space
        public void ReverseInvalid(char letter)
        {
            ReflectorSettings settings = new ReflectorSettings();
            Assert.Equal(letter, settings.Reflect(letter));
        }

        [Theory]
        [InlineData("ABC", "ABC", 'A', 'C', "CBA", 2)]
        [InlineData("ABC", "ABC", 'C', 'A', "CBA", 2)]
        public void SetSubstitutionChange(string characterSet, string substitutions, char from, char to, string newSubstitutions, int substitutionCount)
        {
            ReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(characterSet, settings.CharacterSet);
            Assert.Equal(newSubstitutions, settings.Substitutions);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
        }

        [Theory]
        [InlineData("ABC", "CBA", 'A', 'A', "ABC", 0)]
        public void SetSubstitutionClear(string characterSet, string substitutions, char from, char to, string newSubstitutions, int substitutionCount)
        {
            ReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(characterSet, settings.CharacterSet);
            Assert.Equal(newSubstitutions, settings.Substitutions);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
        }

        [Theory]
        [InlineData("ABC", "CBA", 'A', 'C', "CBA", 2)]
        public void SetSubstitutionExisting(string characterSet, string substitutions, char from, char to, string newSubstitutions, int substitutionCount)
        {
            ReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);
            settings[from] = to;

            Assert.Equal(to, settings[from]);
            Assert.Equal(characterSet, settings.CharacterSet);
            Assert.Equal(newSubstitutions, settings.Substitutions);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
        }

        [Theory]
        [InlineData("ABC", "ABC", 'A', 'Ø', 0)]
        [InlineData("ABC", "ABC", 'Ø', 'A', 0)]
        public void SetSubstitutionInvalid(string characterSet, string substitutions, char from, char to, int substitutionCount)
        {
            ReflectorSettings settings = new ReflectorSettings(characterSet, substitutions);

            Assert.Throws<ArgumentException>("value", () => settings[from] = to);
            Assert.Equal(characterSet, settings.CharacterSet);
            Assert.Equal(substitutions, settings.Substitutions);
            Assert.Equal(substitutionCount, settings.SubstitutionCount);
        }
    }
}