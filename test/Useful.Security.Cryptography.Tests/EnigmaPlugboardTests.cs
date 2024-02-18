// Copyright (c) Andrew Hawkins. All rights reserved.

using Xunit;

namespace Useful.Security.Cryptography.Tests
{
    public class EnigmaPlugboardTests
    {
        public static TheoryData<List<EnigmaPlugboardPair>> InvalidPairs => new()
        {
            {
                new()
                {
                    { new EnigmaPlugboardPair() { From = 'A', To = 'B' } },
                    { new EnigmaPlugboardPair() { From = 'B', To = 'A' } },
                }
            }, // Repeat letters
            {
                new()
                { { new EnigmaPlugboardPair() { From = 'a', To = 'B' } }, }
            }, // Subs incorrect case
            {
                new()
                { { new EnigmaPlugboardPair() { From = 'A', To = 'A' } }, }
            }, // Same letter
        };

        public static TheoryData<List<EnigmaPlugboardPair>, int> ValidPairs => new()
        {
            {
                new List<EnigmaPlugboardPair>()
                {
                    { new EnigmaPlugboardPair() { From = 'A', To = 'B' } },
                    { new EnigmaPlugboardPair() { From = 'C', To = 'D' } },
                },
                2
            },
        };

        [Fact]
        public void CtorEmpty()
        {
            EnigmaPlugboard settings = new();
            Assert.Equal(0, settings.SubstitutionCount);
            Assert.Empty(settings.Substitutions());
            Assert.Equal('A', settings.GetSubstitution('A'));
        }

        [Theory]
        [MemberData(nameof(InvalidPairs))]
        public void CtorSubstitutionsInvalid(IList<EnigmaPlugboardPair> pairs)
            => Assert.Throws<ArgumentException>(nameof(pairs), () => new EnigmaPlugboard(pairs));

        [Fact]
        public void CtorSubstitutionsNull() => Assert.Throws<ArgumentNullException>("pairs", () => new EnigmaPlugboard(null));

        [Theory]
        [MemberData(nameof(ValidPairs))]
        public void CtorSubstitutionsValid(IList<EnigmaPlugboardPair> pairs, int substitutionCount)
        {
            ArgumentNullException.ThrowIfNull(pairs);

            IEnigmaPlugboard plugboard = new EnigmaPlugboard(pairs);
            Assert.Equal(substitutionCount, plugboard.SubstitutionCount);
            Assert.Equal(pairs[0].From, plugboard[pairs[0].To]);
            Assert.Equal(pairs[0].To, plugboard[pairs[0].From]);
        }
    }
}
