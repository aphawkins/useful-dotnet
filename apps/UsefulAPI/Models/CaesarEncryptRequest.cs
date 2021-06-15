// <copyright file="CaesarEncryptRequest.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

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