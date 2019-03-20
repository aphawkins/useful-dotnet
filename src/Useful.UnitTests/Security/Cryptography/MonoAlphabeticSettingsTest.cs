using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Useful.Security.Cryptography;

using System.Diagnostics.Contracts;

namespace UsefulQA
{
    /// <summary>
    /// Summary description for MonoAlphabeticTest
    /// </summary>
    [TestClass]
    public class MonoAlphabeticSettingsTest
    {
        #region ctor
        public MonoAlphabeticSettingsTest()
        {
        }
        #endregion

        #region Fields
        MonoAlphabetic mono = new MonoAlphabetic();
        MonoAlphabeticSettings target;
        int settingsChangedCount;
        bool contractFailed = false;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContextInstance { get; set; }
        #endregion

        #region Methods
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void MonoAlphabeticSettings_ctor()
        {
            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            Assert.IsTrue(target.AllowedLetters.Count == 26);
            Assert.IsTrue(string.Compare(target.CipherName, "MonoAlphabetic") == 0);
            //Assert.IsTrue(target.SubstitutionCount == 0);
            //Contract.ContractFailed += new EventHandler<ContractFailedEventArgs>(Contract_ContractFailed);
        }

        //void Contract_ContractFailed(object sender, ContractFailedEventArgs e)
        //{
        //    contractFailed = true;
        //}

        //[TestMethod]
        //public void MonoAlphabeticSettings_ctor_Key_Null()
        //{
        //    this.contractFailed = false;

        //    target = new MonoAlphabeticSettings(null, mono.IV);
            
        //    if (!this.contractFailed)
        //        Assert.Fail();
        //}

        //[TestMethod]
        //public void MonoAlphabeticSettings_ctor_IV_Null()
        //{
        //    try
        //    {
        //        target = new MonoAlphabeticSettings(mono.Key, null);
        //        Assert.Fail();
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        // Success
        //    }
        //}

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Alphabet_NonEnglish()
        {
            string tempKey = @"VWXYZ|VW XY|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            this.settingsChangedCount = 0;

            target.SetKey(key);

            Assert.IsTrue(target.AllowedLetters.Count == 5);
            Assert.IsTrue(target.SubstitutionCount == 4);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Alphabet_Padded()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ |AB CD|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetKey(key);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Substitutions_Padded()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ| AB CD |False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetKey(key);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Substitutions_Valid()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            this.settingsChangedCount = 0;

            target.SetKey(key);

            Assert.IsTrue(target.AllowedLetters.Count == 26);
            Assert.IsTrue(target.SubstitutionCount == 4);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Substitutions_NotAllowed()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|ØB CD|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetKey(key);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Substitutions_WrongCase()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ | aB CD | False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetKey(key);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Symmetry_ValidAsymmetric()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            this.settingsChangedCount = 0;

            target.SetKey(key);

            Assert.IsTrue(target.AllowedLetters.Count == 26);
            Assert.IsTrue(target.SubstitutionCount == 4);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Symmetry_ValidSymmetric_0()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            this.settingsChangedCount = 0;

            target.SetKey(key);

            Assert.IsTrue(target.AllowedLetters.Count == 26);
            Assert.IsTrue(target.SubstitutionCount == 3);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Symmetry_ValidSymmetric_1()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|True";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            this.settingsChangedCount = 0;

            target.SetKey(key);

            Assert.IsTrue(target.AllowedLetters.Count == 26);
            Assert.IsTrue(target.SubstitutionCount == 4);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Symmetry_WrongCase()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|fAlSe";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            target.SetKey(key);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Symmetry_WrongType()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA|null";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetKey(key);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Symmetry_InvalidSymmetric()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA|True";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetKey(key);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetKey_Symmetry_Padded()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB BC CA| True";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetKey(key);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetSubstitutions_Valid()
        {
            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            this.settingsChangedCount = 0;

            Collection<SubstitutionPair> pairs = new Collection<SubstitutionPair>();
            pairs.Add(new SubstitutionPair('E', 'F'));
            target.SetSubstitutions(pairs);

            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|EF|True";

            Assert.IsTrue(target.AllowedLetters.Count == 26);
            Assert.IsTrue(target.SubstitutionCount == 2);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetSubstitutions_Duplicate_Pair_Symmetric()
        {
            string tempKey = @"ABC|AB|True";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(key, mono.IV);

            Collection<SubstitutionPair> pairs = new Collection<SubstitutionPair>();
            pairs.Add(new SubstitutionPair('A', 'A'));

            try
            {
                target.SetSubstitutions(pairs);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }
        
        [TestMethod()]
        public void MonoAlphabeticSettings_SetSubstitutions_Duplicate_Pair_Asymmetric()
        {
            string tempKey = @"ABC|AB BC|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(key, mono.IV);

            Collection<SubstitutionPair> pairs = new Collection<SubstitutionPair>();
            pairs.Add(new SubstitutionPair('A', 'A'));

            target.SetSubstitutions(pairs);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetSubstitution_Valid()
        {
            string tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD|False";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);

            target = new MonoAlphabeticSettings(mono.Key, mono.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            target.SetKey(key);
            
            this.settingsChangedCount = 0;

            target.SetSubstitution(new SubstitutionPair('E', 'F'));

            tempKey = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF|False";

            Assert.IsTrue(target.AllowedLetters.Count == 26);
            Assert.IsTrue(target.SubstitutionCount == 6);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetSubstitution_NotAllowed()
        {
            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetSubstitution(new SubstitutionPair('Ø', 'F'));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabeticSettings_SetSubstitution_WrongCase()
        {
            target = new MonoAlphabeticSettings(mono.Key, mono.IV);

            try
            {
                target.SetSubstitution(new SubstitutionPair('a', 'F'));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }


        void target_SettingsChanged(object sender, EventArgs e)
        {
            this.settingsChangedCount++;
        }
        #endregion
    }
}
