// <copyright file="CipherViewModelSteps.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Security.Cryptography;
    using TechTalk.SpecFlow;

    [Binding]
    public class CipherViewModelSteps
    {
        private CipherViewModel viewModel;
        private Mock<IRepository<ICipher>> moqRepository;
        private Mock<ICipher> moqCipher;
        private string propertyChanged;

        [Given(@"I have a CipherViewModel")]
        public void GivenIHaveACipherViewModel()
        {
            moqCipher = new Mock<ICipher>();
            moqCipher.Setup(x => x.CipherName).Returns("MoqCipher");
            moqCipher.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("MoqCiphertext");
            moqCipher.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("MoqPlaintext");
            moqRepository = new Mock<IRepository<ICipher>>();
            moqRepository.Setup(x => x.Read()).Returns(() => new List<ICipher>() { moqCipher.Object });
            viewModel = new CipherViewModel(moqRepository.Object);
        }

        [Given(@"my viewmodel plaintext is ""(.*)""")]
        public void GivenMyViewmodelPlaintextIs(string p0)
        {
            viewModel.Plaintext = p0;
        }

        [Given(@"my viewmodel ciphertext is ""(.*)""")]
        public void GivenMyViewmodelCiphertextIs(string p0)
        {
            viewModel.Ciphertext = p0;
        }

        [Given(@"I set the CurrentCipher property")]
        public void GivenISetTheCurrentCipherProperty()
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.CurrentCipher = moqCipher.Object;
            viewModel.CurrentCipher = null;
            propertyChanged = string.Empty;
            viewModel.CurrentCipher = moqCipher.Object;
        }

        [Given(@"I set the CurrentCipherName property")]
        public void GivenISetTheCurrentCipherNameProperty()
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.CurrentCipherName = moqCipher.Object.CipherName;
            viewModel.CurrentCipherName = null;
            propertyChanged = string.Empty;
            viewModel.CurrentCipherName = moqCipher.Object.CipherName;
        }

        [Given(@"I set the Plaintext property")]
        public void GivenISetThePlaintextProperty()
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Plaintext = string.Empty;
            propertyChanged = string.Empty;
            viewModel.Plaintext = "MoqPlaintext";
        }

        [Given(@"I set the Ciphertext property")]
        public void GivenISetTheCiphertextProperty()
        {
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Ciphertext = string.Empty;
            propertyChanged = string.Empty;
            viewModel.Ciphertext = "MoqCiphertext";
        }

        [Given(@"I set the Plaintext property when the event is not subscribed")]
        public void GivenISetThePlaintextPropertyWhenTheEventIsNotSubscribed()
        {
            viewModel.Plaintext = string.Empty;
            propertyChanged = string.Empty;
            viewModel.Plaintext = "MoqPlaintext";
        }

        [When(@"I use the viewmodel to encrypt")]
        public void WhenIUseTheViewmodelToEncrypt()
        {
            viewModel.Encrypt();
        }

        [When(@"I use the viewmodel to decrypt")]
        public void WhenIUseTheViewmodelToDecrypt()
        {
            viewModel.Decrypt();
        }

        [Then(@"the CurrentCipher is not null")]
        public void ThenTheCurrentCipherIsNotNull()
        {
            Assert.IsNotNull(viewModel.CurrentCipher);
        }

        [Then(@"the CurrentCipherName is ""(.*)""")]
        public void ThenTheCurrentCipherNameIs(string p0)
        {
            Assert.AreEqual("MoqCipher", viewModel.CurrentCipherName);
        }

        [Then(@"the CipherNames are ""(.*)""")]
        public void ThenTheCipherNamesAre(string p0)
        {
            CollectionAssert.AreEquivalent(new List<string>() { "MoqCipher" }, viewModel.CipherNames.ToList());
        }

        [Then(@"the viewmodel ciphertext should be ""(.*)""")]
        public void ThenTheViewmodelCiphertextShouldBe(string p0)
        {
            Assert.AreEqual(p0, viewModel.Ciphertext);
        }

        [Then(@"the viewmodel plaintext should be ""(.*)""")]
        public void ThenTheViewmodelPlaintextShouldBe(string p0)
        {
            Assert.AreEqual(p0, viewModel.Plaintext);
        }

        [Then(@"the EncryptCommand is not null")]
        public void ThenTheEncryptCommandIsNotNull()
        {
            Assert.IsNotNull(viewModel.EncryptCommand);

            viewModel.EncryptCommand.Execute(null);
        }

        [Then(@"the CurrentCipher property has changed")]
        public void ThenTheCurrentCipherPropertyHasChanged()
        {
            Assert.AreEqual(";CurrentCipher;CurrentCipherName", propertyChanged);
        }

        [Then(@"the CurrentCipherName property has changed")]
        public void ThenTheCurrentCipherNamePropertyHasChanged()
        {
            Assert.AreEqual(";CurrentCipher;CurrentCipherName", propertyChanged);
        }

        [Then(@"the Plaintext property has changed")]
        public void ThenThePlaintextPropertyHasChanged()
        {
            Assert.AreEqual(";Plaintext", propertyChanged);
        }

        [Then(@"the Plaintext property has not changed")]
        public void ThenThePlaintextPropertyHasNotChanged()
        {
            Assert.AreEqual(string.Empty, propertyChanged);
        }

        [Then(@"the Ciphertext property has changed")]
        public void ThenTheCiphertextPropertyHasChanged()
        {
            Assert.AreEqual(";Ciphertext", propertyChanged);
        }

        [Then(@"the EncryptCommand should be Executable")]
        public void ThenTheEncryptCommandShouldBeExecutable()
        {
            Assert.IsTrue(viewModel.EncryptCommand.CanExecute(null));
            viewModel.EncryptCommand.Execute(null);
        }

        [Then(@"the DecryptCommand should be Executable")]
        public void ThenTheDecryptCommandShouldBeExecutable()
        {
            Assert.IsTrue(viewModel.DecryptCommand.CanExecute(null));
            viewModel.DecryptCommand.Execute(null);
        }

        [Then(@"the EncryptCommand should not be Executable")]
        public void ThenTheEncryptCommandShouldNotBeExecutable()
        {
            Assert.IsFalse(viewModel.EncryptCommand.CanExecute(null));
        }

        [Then(@"the DecryptCommand should not be Executable")]
        public void ThenTheDecryptCommandShouldNotBeExecutable()
        {
            Assert.IsFalse(viewModel.DecryptCommand.CanExecute(null));
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            propertyChanged += ";" + e.PropertyName;
        }
    }
}