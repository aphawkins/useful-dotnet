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
                case ("Caesar"):
                    {
                        cipher = new CaesarCipher();
                        break;
                    }

                case ("Reverse"):
                    {
                        cipher = new ReverseCipher();
                        break;
                    }
                case ("ROT13"):
                    {
                        cipher = new ROT13Cipher();
                        break;
                    }
            }
        }

        [Then(@"the cipher name should be '(.*)'")]
        public void ThenTheCipherNameShouldBe(string p0)
        {
            Assert.AreEqual(p0, this.cipher.CipherName);
            Assert.AreEqual(p0, this.cipher.ToString());
        }

        [Given(@"my plaintext is (.*)")]
        public void GivenMyPlaintextIs(string p0)
        {
            this.plaintext = p0;
        }

        [Given(@"my ciphertext is (.*)")]
        public void GivenMyCiphertextIs(string p0)
        {
            this.ciphertext = p0;
        }

        [When(@"I decrypt")]
        public void WhenIDecrypt()
        {
            this.plaintext = this.cipher.Decrypt(this.ciphertext);
        }

        [When(@"I encrypt")]
        public void WhenIEncrypt()
        {
            this.ciphertext = this.cipher.Encrypt(this.plaintext);
        }

        [Given(@"my Caesar right shift is (.*)")]
        public void GivenMyCaesarRightShiftIs(string p0)
        {
            ((CaesarCipherSettings)((CaesarCipher)this.cipher).Settings).RightShift = int.Parse(p0);
        }

        [Then(@"the ciphertext should be (.*)")]
        public void ThenTheCiphertextShouldBe(string p0)
        {
            Assert.AreEqual(p0, this.ciphertext);
        }

        [Then(@"the plaintext should be (.*)")]
        public void ThenThePlaintextShouldBe(string p0)
        {
            Assert.AreEqual(p0, this.plaintext);
        }
    }
}