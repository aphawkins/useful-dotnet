namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// This is a test class for EnigmaSettingsTest and is intended
    /// to contain all EnigmaSettingsTest Unit Tests
    /// </summary>
    [TestClass]
    public class MonoAlphabeticTransformUnitTests
    {
        private const string Category = "MonoAlphabeticTransform";

        /// <summary>
        /// Dispose the already disposed object.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        [Description("Double dispose")]
        public void MonoAlphabeticTransform_DoubleDispose()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            MonoAlphabetic cipher = new MonoAlphabetic();
            ICryptoTransform transform = cipher.CreateEncryptor(settings.Key.ToArray(), settings.IV.ToArray());
            transform.Dispose();
            transform.Dispose();
        }

        /// <summary>
        /// Can the tranform be reused.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        [Description("Can reuse transform")]
        public void MonoAlphabeticTransform_CanReuseTransform()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            MonoAlphabetic cipher = new MonoAlphabetic();
            ICryptoTransform transform = cipher.CreateEncryptor(settings.Key.ToArray(), settings.IV.ToArray());
            Assert.IsFalse(transform.CanReuseTransform);
        }

        /// <summary>
        /// Basic decipher.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        public void MonoAlphabeticTransform_Decipher()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            settings['A'] = 'B';
            MonoAlphabeticTransform target = new MonoAlphabeticTransform(settings.Key.ToArray(), settings.IV.ToArray(), CipherTransformMode.Encrypt);
            Assert.AreEqual('A', target.Decipher('B'));
        }

        /////// <summary>
        /////// Decipher an unallowed letter.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(Category)]
        ////public void MonoAlphabeticTransform_Decipher_Unallowed()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    MonoAlphabeticTransform target = new MonoAlphabeticTransform(settings.Key.ToArray(), settings.IV.ToArray(), CipherTransformMode.Encrypt);
        ////    Assert.AreEqual('a', target.Decipher('a'));
        ////}

        /// <summary>
        /// Basic encipher.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        public void MonoAlphabeticTransform_Encipher()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            settings['A'] = 'B';
            MonoAlphabeticTransform target = new MonoAlphabeticTransform(settings.Key.ToArray(), settings.IV.ToArray(), CipherTransformMode.Encrypt);
            Assert.AreEqual('A', target.Encipher('B'));
        }

        /// <summary>
        /// Basic encipher.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        public void MonoAlphabeticTransform_TransformString_Encipher()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            settings['A'] = 'B';
            MonoAlphabeticTransform target = new MonoAlphabeticTransform(settings.Key.ToArray(), settings.IV.ToArray(), CipherTransformMode.Encrypt);
            Assert.AreEqual("BBB", target.TransformString("AAA"));
        }

        /// <summary>
        /// Basic decipher.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        public void MonoAlphabeticTransform_TransformString_Decipher()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            settings['A'] = 'B';
            MonoAlphabeticTransform target = new MonoAlphabeticTransform(settings.Key.ToArray(), settings.IV.ToArray(), CipherTransformMode.Decrypt);
            Assert.AreEqual("AAA", target.TransformString("BBB"));
        }

        /////// <summary>
        /////// Encipher an unallowed letter.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(Category)]
        ////public void MonoAlphabeticTransform_Encipher_Unallowed()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    MonoAlphabeticTransform transform = new MonoAlphabeticTransform(settings.Key.ToArray(), settings.IV.ToArray(), CipherTransformMode.Encrypt);
        ////    Assert.AreEqual('a', transform.Encipher('a'));
        ////}

        /// <summary>
        /// Basic encipher.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        public void MonoAlphabeticTransform_TransformBlock_Encipher()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            settings['A'] = 'B';
            MonoAlphabetic cipher = new MonoAlphabetic();
            ICryptoTransform transform = cipher.CreateEncryptor(settings.Key.ToArray(), settings.IV.ToArray());

            byte[] inputBuffer = Encoding.Unicode.GetBytes("HELLO WORLD");
            byte[] outputBuffer = new byte[transform.OutputBlockSize];

            Assert.AreEqual(transform.OutputBlockSize, transform.TransformBlock(inputBuffer, 0, transform.InputBlockSize, outputBuffer, 0));
            Assert.AreEqual("H", Encoding.Unicode.GetString(outputBuffer));
        }

        /// <summary>
        /// Basic decipher.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        public void MonoAlphabeticTransform_TransformBlock_Decipher()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            settings['A'] = 'B';
            MonoAlphabetic cipher = new MonoAlphabetic();
            ICryptoTransform transform = cipher.CreateDecryptor(settings.Key.ToArray(), settings.IV.ToArray());

            byte[] inputBuffer = Encoding.Unicode.GetBytes("HELLO WORLD");
            byte[] outputBuffer = new byte[transform.OutputBlockSize];

            Assert.AreEqual(transform.OutputBlockSize, transform.TransformBlock(inputBuffer, 0, transform.InputBlockSize, outputBuffer, 0));
            Assert.AreEqual("H", Encoding.Unicode.GetString(outputBuffer));
        }

        /// <summary>
        /// Basic decipher.
        /// </summary>
        [TestMethod]
        [TestCategory(Category)]
        public void MonoAlphabeticTransform_TransformFinalBlock_Encipher()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
            settings['A'] = 'B';
            MonoAlphabetic cipher = new MonoAlphabetic();
            ICryptoTransform transform = cipher.CreateEncryptor(settings.Key.ToArray(), settings.IV.ToArray());

            byte[] inputBuffer = Encoding.Unicode.GetBytes("HELLO WORLD");
            Assert.AreEqual(string.Empty, Encoding.Unicode.GetString(transform.TransformFinalBlock(inputBuffer, 0, transform.InputBlockSize)));
        }
    }
}