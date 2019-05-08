using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Useful.Security.Cryptography;
using System.IO;

namespace TestProject1
{


    /// <summary>
    ///This is a test class for EnigmaSettingsTest and is intended
    ///to contain all EnigmaSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnigmaTest
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
        ///A test for EnigmaSettings Constructor
        ///</summary>
        [TestMethod()]
        public void Enigma_ctor()
        {
            Enigma target = new Enigma();
        }

        [TestMethod()]
        public void Enigma_CreateEncryptor()
        {
            Enigma target = new Enigma();
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
        public void Enigma_Basic()
        {
            Enigma target = new Enigma();

            TestTarget(target, @"Military|I II III|", @"A A A", @"HELLOWORLD", @"MFNCZBBFZM", @"K A A");
        }

        private void TestTarget(SymmetricAlgorithm target, string key, string iv, string input, string output, string newIv)
        {
            target.Key = Encoding.Unicode.GetBytes(key);
            target.IV = Encoding.Unicode.GetBytes(iv);
            TestTargetStream(target, input, output);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.Key), key, false) == 0);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.IV), newIv, false) == 0);
  
            target.Key = Encoding.Unicode.GetBytes(key);
            target.IV = Encoding.Unicode.GetBytes(iv);
            TestTargetString(target, input, output);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.Key), key, false) == 0);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.IV), newIv, false) == 0);
        }

        private void TestTargetStream(SymmetricAlgorithm target, string input, string output)
        {
            using (MemoryStream inputStream = new MemoryStream(Encoding.Unicode.GetBytes(input)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    CipherMethods.DoCipher(target, CipherTransformMode.Encrypt, inputStream, outputStream);

                    outputStream.Flush();

                    Assert.IsTrue(string.Compare(new string(Encoding.Unicode.GetChars(outputStream.ToArray())), output, false) == 0);
                }
            }
        }

        private void TestTargetString(SymmetricAlgorithm target, string input, string output)
        {
            string enciphered = CipherMethods.DoCipher(target, CipherTransformMode.Encrypt, input);
            Assert.IsTrue(string.Compare(enciphered, output, false) == 0);
        }

        [TestMethod()]
        public void Enigma_Notches()
        {
            Enigma target = new Enigma();

            TestTarget(target, @"Military|I II III|", @"Q E V", @"HELLO WORLD", @"UXEOERTMHJ", @"A F W");
        }

        [TestMethod()]
        public void Enigma_Spaces()
        {
            Enigma target = new Enigma();

            TestTarget(target, @"Military|I II III|", @"A A A", @"HELLO WORLD", @"MFNCZBBFZM", @"K A A");
        }

		[TestMethod()]
		public void Enigma_Disallowed_Chars()
		{
			Enigma target = new Enigma();

			TestTarget(target, @"Military|I II III|", @"A A A", @"HELLOÅÅÅÅÅWORLD", @"MFNCZBBFZM", @"K A A");
		}

        [TestMethod()]
        public void Enigma_Mixed_Case()
        {
            Enigma target = new Enigma();

            TestTarget(target, @"Military|I II III|", @"A A A", @"HeLlOwOrLd", @"MFNCZBBFZM", @"K A A");
        }

		[TestMethod()]
		public void Enigma_Backspace()
		{
			Enigma target = new Enigma();

			TestTarget(target, @"Military|I II III|", @"A A A", @"HELLOWORLD", @"MFNCZBBFZM", @"K A A");

			// target.
		}

            #region Test Clear
            // TODO:
            // target.Clear();
            #endregion

        #endregion
    }
}
