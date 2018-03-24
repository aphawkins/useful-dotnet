// <copyright file="ICipherSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class ICipherSteps
    {
        private ICipher cipher;
        private string plaintext;
        private string ciphertext;

        [Given(@"I have a '(.*)' cipher")]
        public void GivenIHaveACipher(string p0)
        {
            switch (p0)
            {
                case "Caesar":
                    {
                        cipher = new CaesarCipher();
                        break;
                    }

                case "Reverse":
                    {
                        cipher = new ReverseCipher();
                        break;
                    }

                case "ROT13":
                    {
                        cipher = new ROT13Cipher();
                        break;
                    }
            }
        }

        [Then(@"the cipher name should be '(.*)'")]
        public void ThenTheCipherNameShouldBe(string p0)
        {
            Assert.AreEqual(p0, cipher.CipherName);
            Assert.AreEqual(p0, cipher.ToString());
        }

        [Given(@"my plaintext is (.*)")]
        public void GivenMyPlaintextIs(string p0)
        {
            plaintext = p0;
        }

        [Given(@"my ciphertext is (.*)")]
        public void GivenMyCiphertextIs(string p0)
        {
            ciphertext = p0;
        }

        [When(@"I decrypt")]
        public void WhenIDecrypt()
        {
            plaintext = cipher.Decrypt(ciphertext);
        }

        [When(@"I encrypt")]
        public void WhenIEncrypt()
        {
            ciphertext = cipher.Encrypt(plaintext);
        }

        [Given(@"my Caesar right shift is (.*)")]
        public void GivenMyCaesarRightShiftIs(string p0)
        {
            ((CaesarCipherSettings)((CaesarCipher)cipher).Settings).RightShift = int.Parse(p0);
        }

        [Then(@"the ciphertext should be (.*)")]
        public void ThenTheCiphertextShouldBe(string p0)
        {
            Assert.AreEqual(p0, ciphertext);
        }

        [Then(@"the plaintext should be (.*)")]
        public void ThenThePlaintextShouldBe(string p0)
        {
            Assert.AreEqual(p0, plaintext);
        }
    }
}