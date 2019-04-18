// <copyright file="MonoAlphabeticUnitTests.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>Tests the monoalphabetic substitution cipher.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// This is a test class for MonoAlphabetic and is intended to contain all the Unit Tests.
    /// </summary>
    [TestClass]
    [DebuggerDisplay("{ToString()}")]
    public sealed class MonoAlphabeticUnitTests
    {
        /// <summary>
        /// Create decryptor test.
        /// </summary>
        [TestMethod]
        public void MonoAlphabetic_CreateDecryptor()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            ICryptoTransform transformer = target.CreateDecryptor();
        }

        /// <summary>
        /// Create decryptor test with null IV.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MonoAlphabetic_CreateDecryptor_IvNull()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            ICryptoTransform transformer = target.CreateDecryptor(target.Key, null);
        }

        /// <summary>
        /// Create decryptor test with null Key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MonoAlphabetic_CreateDecryptor_KeyNull()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            ICryptoTransform transformer = target.CreateDecryptor(null, target.IV);
        }

        /// <summary>
        /// Create encryptor test.
        /// </summary>
        [TestMethod]
        public void MonoAlphabetic_CreateEncryptor()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            ICryptoTransform transformer = target.CreateEncryptor();
        }

        /// <summary>
        /// Create encryptor test with null IV.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MonoAlphabetic_CreateEncryptor_IvNull()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            ICryptoTransform transformer = target.CreateEncryptor(target.Key, null);
        }

        /// <summary>
        /// Create encryptor test with null Key.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MonoAlphabetic_CreateEncryptor_KeyNull()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            ICryptoTransform transformer = target.CreateEncryptor(null, target.IV);
        }

        /// <summary>
        /// A test for MonoAlphabetic Constructor
        /// </summary>
        [TestMethod]
        public void MonoAlphabetic_ctor()
        {
            MonoAlphabetic target = new MonoAlphabetic();
        }

        /// <summary>
        /// File encryption test.
        /// </summary>
        [TestMethod]
        public void MonoAlphabetic_File_Encrypt()
        {
            MonoAlphabetic target = new MonoAlphabetic();

            CipherTestUtilities.TestFile(target, CipherTransformMode.Encrypt, "HELLO WORLD");
        }

        /// <summary>
        /// Generate Key.
        /// </summary>
        [TestMethod]
        public void MonoAlphabetic_GenerateIv()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            byte[] originalIv = target.IV;
            target.GenerateIV();
            byte[] newIv = target.IV;

            // IV is always the same
            Assert.IsTrue(originalIv.SequenceEqual(newIv));
        }

        /// <summary>
        /// Generate Key.
        /// </summary>
        [TestMethod]
        public void MonoAlphabetic_GenerateKey()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            byte[] originalKey = target.Key;
            target.GenerateKey();
            byte[] newKey = target.Key;
            Assert.IsFalse(originalKey.SequenceEqual(newKey));
        }
    }
}