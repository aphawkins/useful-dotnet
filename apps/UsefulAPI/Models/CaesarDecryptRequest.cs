// Copyright (c) Andrew Hawkins. All rights reserved.

namespace UsefulAPI
{
    public class CaesarDecryptRequest : DecryptRequest
    {
        public int RightShift
        {
            get;
            set;
        }
    }
}