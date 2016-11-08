namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TechTalk.SpecFlow;

    [Binding]
    public class CipherRepositorySteps
    {
        private IRepository<ICipher> repository;
        private Mock<ICipher> moqCipher;

        [Given(@"I have a CipherRepository")]
        public void GivenIHaveACipherRepository()
        {
            this.moqCipher = new Mock<ICipher>();
            this.moqCipher.Setup(x => x.CipherName).Returns("MoqCipherName");
            this.repository = new CipherRepository();
        }

        [When(@"I create a new cipher")]
        public void WhenICreateANewCipher()
        {
            this.repository.Create(moqCipher.Object);
        }

        [When(@"I update a cipher")]
        public void WhenIUpdateACipher()
        {
            this.repository.Update(moqCipher.Object);
        }

        [When(@"I delete a cipher")]
        public void WhenIDeleteACipher()
        {
            this.repository.Delete(moqCipher.Object);
        }

        [When(@"I load the ciphers")]
        public void WhenILoadTheCiphers()
        {
            this.repository = CipherRepository.Create();
        }

        [When(@"I SetCurrentItem")]
        public void WhenISetCurrentItem()
        {
            this.repository.SetCurrentItem(x => x.CipherName == "MoqCipherName");
        }

        [Then(@"the CurrentItem will be set")]
        public void ThenTheCurrentItemWillBeSet()
        {
            Assert.AreEqual("MoqCipherName", this.repository.CurrentItem.CipherName); 
        }

        [Then(@"the CurrentItem will not be set")]
        public void ThenTheCurrentItemWillNotBeSet()
        {
            Assert.IsNull(this.repository.CurrentItem);
        }

        [Then(@"there should be ""(.*)"" ciphers")]
        public void ThenThereShouldBeCiphers(int p0)
        {
            Assert.AreEqual(p0, this.repository.Read().Count);
        }
    }
}