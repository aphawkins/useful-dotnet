// Copyright (c) Andrew Hawkins. All rights reserved.

using Xunit;

namespace Useful.Drawing.Tests
{
    public class ColourConverterTests
    {
        [Theory]
        [InlineData(0xFF000000, 0)]
        [InlineData(0xFFFFFFFF, 255)]
        [InlineData(0xFF240000, 1 << 5)]
        [InlineData(0xFF480000, 2 << 5)]
        [InlineData(0xFF6D0000, 3 << 5)]
        [InlineData(0xFF910000, 4 << 5)]
        [InlineData(0xFFB60000, 5 << 5)]
        [InlineData(0xFFDA0000, 6 << 5)]
        [InlineData(0xFFFF0000, 7 << 5)]
        [InlineData(0xFF002400, 1 << 2)]
        [InlineData(0xFF004800, 2 << 2)]
        [InlineData(0xFF006D00, 3 << 2)]
        [InlineData(0xFF009100, 4 << 2)]
        [InlineData(0xFF00B600, 5 << 2)]
        [InlineData(0xFF00DA00, 6 << 2)]
        [InlineData(0xFF00FF00, 7 << 2)]
        [InlineData(0xFF000054, 1)]
        [InlineData(0xFF0000AA, 2)]
        [InlineData(0xFF0000FF, 3)]
        [InlineData(0xFF4848AA, 74)]
        public void Convert8BitToARGBColorTests(uint argb, byte rrrgggbb)
            => Assert.Equal(unchecked((int)argb), ColourConverter.Convert8BitToARGBColor(rrrgggbb));
    }
}