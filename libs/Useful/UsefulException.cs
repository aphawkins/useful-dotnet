// Copyright (c) Andrew Hawkins. All rights reserved.

namespace Useful;

public class UsefulException : Exception
{
    public UsefulException(string message)
        : base(message)
    {
    }

    public UsefulException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    public UsefulException()
    {
    }
}
