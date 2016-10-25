namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    [Binding]
    public class CipherRepositorySteps
    {
        private CipherRepository repository;

        [Given(@"I have a CipherRepository")]
        public void GivenIHaveACipherRepository()
        {
            this.repository = new CipherRepository();
        }

        [Then(@"there should be ""(.*)"" ciphers")]
        public void ThenThereShouldBeCiphers(int p0)
        {
            Assert.AreEqual(p0, this.repository.GetCiphers().Count);
        }
    }
}