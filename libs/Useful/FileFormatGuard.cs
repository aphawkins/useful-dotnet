// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful
{
    public static class FileFormatGuard
    {
        public static void Equal(string expected, string actual, string fieldName)
        {
            if (expected != actual)
            {
                throw new FileFormatException($"{fieldName} incorrect. Expected: {expected}, Actual: {actual}");
            }
        }

        public static void Equal(int expected, int actual, string fieldName)
        {
            if (expected != actual)
            {
                throw new FileFormatException($"{fieldName} incorrect. Expected: {expected}, Actual: {actual}");
            }
        }

        public static void Range(int minExpected, int maxExpected, int actual, string fieldName)
        {
            if (actual < minExpected || actual > maxExpected)
            {
                throw new FileFormatException($"{fieldName} incorrect. Expected range: {minExpected}-{maxExpected}, Actual: {actual}");
            }
        }

        public static void ReadBytes(int count)
        {
            if (count < 1)
            {
                throw new FileFormatException("Failed to read file.");
            }
        }
    }
}
