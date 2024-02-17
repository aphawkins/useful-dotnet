// Copyright (c) Andrew Hawkins. All rights reserved.

namespace UsefulAPI
{
    public class CaesarEncryptRequest : EncryptRequest
    {
        public int RightShift
        {
            get;
            set;
        }
    }
}