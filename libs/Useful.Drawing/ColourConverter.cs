// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful.Drawing
{
    /// <summary>
    /// Converts colours from one format to another.
    /// </summary>
    public static class ColourConverter
    {
        /// <summary>
        /// Converts 8bit RRRGGGBB to ARGB 32bit colour.
        /// </summary>
        /// <param name="color8Bit">The 8bit colour code.</param>
        /// <returns>The 32bit colour code.</returns>
        public static int Convert8BitToARGBColor(byte color8Bit)
        {
            int red = Math.Clamp((int)Math.Round((((color8Bit >> 5) & 7) * (256f / 7)) - 1), 0, 255);
            int green = Math.Clamp((int)Math.Round((((color8Bit >> 2) & 7) * (256f / 7)) - 1), 0, 255);
            int blue = Math.Clamp((int)Math.Round(((color8Bit & 3) * (256f / 3)) - 1), 0, 255);

            return (0xFF << 24) | (red << 16) | (green << 8) | blue;
        }
    }
}