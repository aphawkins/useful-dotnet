// <copyright file="CipherControllerSteps.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Security.Cryptography;
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
            moqCipher = new Mock<ICipher>();
            moqCipher.Setup(x => x.CipherName).Returns("MoqCipherName");
            moqCipher.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("MoqCiphertext");
            moqRepository = new Mock<IRepository<ICipher>>();
            moqRepository.Setup(x => x.Read()).Returns(() => new List<ICipher>() { moqCipher.Object });
            moqRepository.Setup(x => x.CurrentItem).Returns(() => moqCipher.Object);
            moqView = new Mock<ICipherView>();
            moqSettingsView = new Mock<ICipherSettingsView>();
            controller = new CipherController(moqRepository.Object, moqView.Object);
        }

        [When(@"I load the view")]
        public void WhenILoadTheView()
        {
            controller.LoadView();
        }

        [When(@"I encrypt ""(.*)""")]
        public void WhenIEncrypt(string p0)
        {
            controller.Encrypt(p0);
        }

        [When(@"I decrypt ""(.*)""")]
        public void WhenIDecrypt(string p0)
        {
            controller.Decrypt(p0);
        }

        [When(@"I select a cipher")]
        public void WhenISelectACipher()
        {
            controller.SelectCipher(moqCipher.Object, moqSettingsView.Object);
        }

        [Then(@"the repository's Current Item will be set")]
        public void ThenTheRepositorySCurrentItemWillBeSet()
        {
            moqRepository.Verify(x => x.SetCurrentItem(It.IsAny<Func<ICipher, bool>>()));
        }

        [Then(@"the view will be initialized")]
        public void ThenTheViewWillBeInitialized()
        {
            moqView.Verify(x => x.Initialize());
        }

        [Then(@"the view's Plaintext will be called")]
        public void ThenTheViewsPlaintextWillBeCalled()
        {
            moqView.Verify(x => x.ShowPlaintext(It.IsAny<string>()));
        }

        [Then(@"the view's Ciphertext will be called")]
        public void ThenTheViewsCiphertextWillBeCalled()
        {
            moqView.Verify(x => x.ShowCiphertext(It.IsAny<string>()));
        }
    }
}