using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Useful.Security.Cryptography;
using System.IO;

namespace UsefulQA
{
    /// <summary>
    ///This is a test class for EnigmaSettingsTest and is intended
    ///to contain all EnigmaSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MonoAlphabeticTest
    {
        #region Fields
        #endregion

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContextInstance { get; set; }
        #endregion

        #region Methods
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for MonoAlphabetic Constructor
        ///</summary>
        [TestMethod()]
        public void MonoAlphabetic_ctor()
        {
            MonoAlphabetic target = new MonoAlphabetic();
        }

        [TestMethod()]
        public void MonoAlphabetic_CreateEncryptor()
        {
            MonoAlphabetic target = new MonoAlphabetic();
            try
            {
                ICryptoTransform transformer = target.CreateEncryptor();
            }
            catch (CryptographicException)
            {
                // Expected
            }
        }

        [TestMethod()]
        public void MonoAlphabetic_Basic()
        {
            MonoAlphabetic target = new MonoAlphabetic();

            CipherTestUtils.TestTarget(target, @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF GH|True", @"", @"HELLOWORLD", @"GFLLOWORLC", @"");
        }

        [TestMethod()]
        public void MonoAlphabetic_Spaces()
        {
            MonoAlphabetic target = new MonoAlphabetic();

            CipherTestUtils.TestTarget(target, @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF GH|True", @"", @"HELLO WORLD", @"GFLLO WORLC", @"");
        }

        [TestMethod()]
        public void MonoAlphabetic_Disallowed_Chars()
        {
            MonoAlphabetic target = new MonoAlphabetic();

            CipherTestUtils.TestTarget(target, @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF GH|True", @"", @"HELLOÅÅÅÅÅWORLD", @"GFLLOÅÅÅÅÅWORLC", @"");
        }

        [TestMethod()]
        public void MonoAlphabetic_Mixed_Case()
        {
            MonoAlphabetic target = new MonoAlphabetic();

            CipherTestUtils.TestTarget(target, @"ABCDEFGHIJKLMNOPQRSTUVWXYZ|AB CD EF GH|True", @"", @"HeLlOwOrLd", @"GFLLOWORLC", @"");
        }

        [TestMethod()]
        public void MonoAlphabetic_Symmetric()
        {
            MonoAlphabetic target = new MonoAlphabetic();

           CipherTestUtils.TestTarget(target, @"ABC|AB|True", @"", @"ABC", @"BAC", @"");
        }

        [TestMethod()]
        public void MonoAlphabetic_Symmetric_Not()
        {
            MonoAlphabetic target = new MonoAlphabetic();

            CipherTestUtils.TestTarget(target, @"ABC|AB BC|False", @"", @"ABC", @"BCA", @"");
        }

        [TestMethod()]
        public void MonoAlphabetic_File()
        {
            MonoAlphabetic target = new MonoAlphabetic();

            CipherTestUtils.TestFile(target, "HELLO WORLD");
        }

        //[TestMethod()]
        //public void Enigma_Backspace()
        //{
        //    Enigma target = new Enigma();

        //    TestTarget(target, @"Military|I II III|", @"A A A", @"HELLOWORLD", @"MFNCZBBFZM", @"K A A");

        //    // target.
        //}

            #region Test Clear
            // TODO:
            // target.Clear();
            #endregion

        #endregion
    }
}
