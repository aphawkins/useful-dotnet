namespace Useful.UI.ViewModels
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Security.Cryptography;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using TechTalk.SpecFlow;

    [Binding]
    public class CipherViewModelSteps
    {
        private CipherViewModel viewModel;
        private Mock<ICipherRepository> moqRepository;
        private Mock<ICipher> moqCipher;
        private string propertyChanged;

        [Given(@"I have a CipherViewModel")]
        public void GivenIHaveACipherViewModel()
        {
            this.moqCipher = new Mock<ICipher>();
            this.moqCipher.Setup(x => x.CipherName).Returns("MoqCipher");
            this.moqCipher.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("MoqCiphertext");
            this.moqRepository = new Mock<ICipherRepository>();
            this.moqRepository.Setup(x => x.GetCiphers()).Returns(() => new List<ICipher>() { this.moqCipher.Object });
            this.viewModel = new CipherViewModel(this.moqRepository.Object);
            this.viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.propertyChanged += ";" + e.PropertyName;
        }

        [Given(@"my viewmodel plaintext is ""(.*)""")]
        public void GivenMyViewmodelPlaintextIs(string p0)
        {
            this.viewModel.Plaintext = p0;
        }

        [Given(@"I set the CurrentCipher property")]
        public void GivenISetTheCurrentCipherProperty()
        {
            this.viewModel.CurrentCipher = this.moqCipher.Object;
            this.viewModel.CurrentCipher = null;
            this.propertyChanged = string.Empty;
            this.viewModel.CurrentCipher = this.moqCipher.Object;
        }

        [Given(@"I set the CurrentCipherName property")]
        public void GivenISetTheCurrentCipherNameProperty()
        {
            this.viewModel.CurrentCipherName = this.moqCipher.Object.CipherName;
            this.viewModel.CurrentCipherName = null;
            this.propertyChanged = string.Empty;
            this.viewModel.CurrentCipherName = this.moqCipher.Object.CipherName;
        }

        [Given(@"I set the Plaintext property")]
        public void GivenISetThePlaintextProperty()
        {
            this.viewModel.Plaintext = string.Empty;
            this.propertyChanged = string.Empty;
            this.viewModel.Plaintext = "MoqPlaintext";
        }

        [Given(@"I set the Ciphertext property")]
        public void GivenISetTheCiphertextProperty()
        {
            this.viewModel.Ciphertext = string.Empty;
            this.propertyChanged = string.Empty;
            this.viewModel.Ciphertext = "MoqCiphertext";
        }

        [When(@"I use the viewmodel to encrypt")]
        public void WhenIUseTheViewmodelToEncrypt()
        {
            this.viewModel.Encrypt();
        }

        [Then(@"the CurrentCipher is not null")]
        public void ThenTheCurrentCipherIsNotNull()
        {
            Assert.IsNotNull(this.viewModel.CurrentCipher);
        }

        [Then(@"the CurrentCipherName is ""(.*)""")]
        public void ThenTheCurrentCipherNameIs(string p0)
        {
            Assert.AreEqual("MoqCipher", this.viewModel.CurrentCipherName);
        }

        [Then(@"the CipherNames are ""(.*)""")]
        public void ThenTheCipherNamesAre(string p0)
        {
            CollectionAssert.AreEquivalent(new List<string>() { "MoqCipher" }, this.viewModel.CipherNames.ToList());
        }

        [Then(@"the viewmodel ciphertext should be ""(.*)""")]
        public void ThenTheViewmodelCiphertextShouldBe(string p0)
        {
            Assert.AreEqual(p0, this.viewModel.Ciphertext);
        }

        [Then(@"the EncryptCommand is not null")]
        public void ThenTheEncryptCommandIsNotNull()
        {
            Assert.IsNotNull(this.viewModel.EncryptCommand);
        }

        [Then(@"the CurrentCipher property has changed")]
        public void ThenTheCurrentCipherPropertyHasChanged()
        {
            Assert.AreEqual(";CurrentCipher;CurrentCipherName", this.propertyChanged);
        }

        [Then(@"the CurrentCipherName property has changed")]
        public void ThenTheCurrentCipherNamePropertyHasChanged()
        {
            Assert.AreEqual(";CurrentCipher;CurrentCipherName", this.propertyChanged);
        }

        [Then(@"the Plaintext property has changed")]
        public void ThenThePlaintextPropertyHasChanged()
        {
            Assert.AreEqual(";Plaintext", this.propertyChanged);
        }

        [Then(@"the Ciphertext property has changed")]
        public void ThenTheCiphertextPropertyHasChanged()
        {
            Assert.AreEqual(";Ciphertext", this.propertyChanged);
        }
    }
}