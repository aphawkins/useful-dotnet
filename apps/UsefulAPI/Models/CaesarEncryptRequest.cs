// Copyright (c) Andrew Hawkins. All rights reserved.

namespace UsefulAPI.Models;

public class CaesarEncryptRequest : EncryptRequest
{
    public int RightShift { get; set; }
}
