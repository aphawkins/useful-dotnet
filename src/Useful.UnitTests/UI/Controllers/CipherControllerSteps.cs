namespace Useful.UI.Controllers
{
    using System;
    using Moq;
    using Security.Cryptography;
    using TechTalk.SpecFlow;
    using Views;

    [Binding]
    public class CipherControllerSteps
    {
        private Mock<ICipher> moqCipher;
        private Mock<ICipherView> moqView;
        private CipherController controller;

        [Given(@"I have a CipherController")]
        public void GivenIHaveACipherController()
        {
            this.moqCipher = new Mock<ICipher>();
            this.moqView = new Mock<ICipherView>();
            this.controller = new CipherController(this.moqCipher.Object, this.moqView.Object);
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

        [Then(@"the view's CipherName will be called")]
        public void ThenTheViewsCipherNameWillBeCalled()
        {
            this.moqView.Verify(x => x.ShowCiphername(It.IsAny<string>()));
        }
        
        [Then(@"the view will be initialized")]
        public void ThenTheViewWillBeInitialized()
        {
            this.moqView.Verify(x => x.Initialize());
        }

        [Then(@"the view's Plaintext will be called")]
        public void ThenTheViewSPlaintextWillBeCalled()
        {
            this.moqView.Verify(x => x.ShowPlaintext(It.IsAny<string>()));
        }

        [Then(@"the view's Ciphertext will be called")]
        public void ThenTheViewSCiphertextWillBeCalled()
        {
            this.moqView.Verify(x => x.ShowCiphertext(It.IsAny<string>()));
        }
    }
}