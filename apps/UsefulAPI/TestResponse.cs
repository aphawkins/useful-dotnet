// <copyright file="TestResponse.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulAPI
{
    public class TestResponse : ITest
    {
        public string? Ciphertext
        {
            get;
            set;
        }
    }
}