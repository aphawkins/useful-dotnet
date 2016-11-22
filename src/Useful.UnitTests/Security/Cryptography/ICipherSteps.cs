namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class ICipherSteps
    {
        ICipher cipher;
        string plaintext;
        string ciphertext;

        [Given(@"I have a ""(.*)"" cipher")]
        public void GivenIHaveACipher(string p0)
        {
            switch (p0)
            {
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

        [Then(@"the cipher name should be ""(.*)""")]
        public void ThenTheCipherNameShouldBe(string p0)
        {
            Assert.AreEqual(this.cipher.CipherName, p0);
        }

        [Given(@"my plaintext is ""(.*)""")]
        public void GivenMyPlaintextIs(string p0)
        {
            this.plaintext = p0;
        }

        [Given(@"my ciphertext is ""(.*)""")]
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
        
        [Then(@"the ciphertext should be ""(.*)""")]
        public void ThenTheCiphertextShouldBe(string p0)
        {
            Assert.AreEqual(p0, this.ciphertext);
        }

        [Then(@"the plaintext should be ""(.*)""")]
        public void ThenThePlaintextShouldBe(string p0)
        {
            Assert.AreEqual(p0, this.plaintext);
        }
    }
}