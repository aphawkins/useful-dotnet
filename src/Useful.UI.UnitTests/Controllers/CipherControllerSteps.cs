﻿namespace Useful.UI.Controllers
{
    using Moq;
    using Security.Cryptography;
    using System;
    using System.Collections.Generic;
    using TechTalk.SpecFlow;
    using Views;

    [Binding]
    public class CipherControllerSteps
    {
        private Mock<ICipher> moqCipher;
        private Mock<IRepository<ICipher>> moqRepository;
        private Mock<ICipherView> moqView;
        private Mock<ICipherSettingsView> moqSettingsView;
        private CipherController controller;

        [Given(@"I have a CipherController")]
        public void GivenIHaveACipherController()
        {
            this.moqCipher = new Mock<ICipher>();
            this.moqCipher.Setup(x => x.CipherName).Returns("MoqCipherName");
            this.moqCipher.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("MoqCiphertext");
            this.moqRepository = new Mock<IRepository<ICipher>>();
            this.moqRepository.Setup(x => x.Read()).Returns(() => new List<ICipher>() { this.moqCipher.Object });
            this.moqRepository.Setup(x => x.CurrentItem).Returns(() => this.moqCipher.Object);
            this.moqView = new Mock<ICipherView>();
            this.moqSettingsView = new Mock<ICipherSettingsView>();
            this.controller = new CipherController(this.moqRepository.Object, this.moqView.Object);
        }

        [When(@"I load the view")]
        public void WhenILoadTheView()
        {
            this.controller.LoadView();
        }

        [When(@"I encrypt ""(.*)""")]
        public void WhenIEncrypt(string p0)
        {
            this.controller.Encrypt(p0);
        }

        [When(@"I decrypt ""(.*)""")]
        public void WhenIDecrypt(string p0)
        {
            this.controller.Decrypt(p0);
        }

        [When(@"I select a cipher")]
        public void WhenISelectACipher()
        {
            this.controller.SelectCipher(moqCipher.Object, moqSettingsView.Object);
        }

        [Then(@"the repository's Current Item will be set")]
        public void ThenTheRepositorySCurrentItemWillBeSet()
        {
            this.moqRepository.Verify(x => x.SetCurrentItem(It.IsAny<Func<ICipher, bool>>()));
        }

        [Then(@"the view will be initialized")]
        public void ThenTheViewWillBeInitialized()
        {
            this.moqView.Verify(x => x.Initialize());
        }

        [Then(@"the view's Plaintext will be called")]
        public void ThenTheViewsPlaintextWillBeCalled()
        {
            this.moqView.Verify(x => x.ShowPlaintext(It.IsAny<string>()));
        }

        [Then(@"the view's Ciphertext will be called")]
        public void ThenTheViewsCiphertextWillBeCalled()
        {
            this.moqView.Verify(x => x.ShowCiphertext(It.IsAny<string>()));
        }
    }
}