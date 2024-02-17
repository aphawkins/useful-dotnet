// Copyright (c) Andrew Hawkins. All rights reserved.

namespace UsefulAPI.Models
{
    public class CaesarDecryptRequest : DecryptRequest
    {
        public int RightShift { get; set; }
    }
}
