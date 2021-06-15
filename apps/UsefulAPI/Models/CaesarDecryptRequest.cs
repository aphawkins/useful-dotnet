// <copyright file="CaesarDecryptRequest.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

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