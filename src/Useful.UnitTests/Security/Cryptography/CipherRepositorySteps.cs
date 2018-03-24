// <copyright file="CipherRepositorySteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
            moqCipher = new Mock<ICipher>();
            moqCipher.Setup(x => x.CipherName).Returns("MoqCipherName");
            repository = new CipherRepository();
        }

        [When(@"I create a new cipher")]
        public void WhenICreateANewCipher()
        {
            repository.Create(moqCipher.Object);
        }

        [When(@"I update a cipher")]
        public void WhenIUpdateACipher()
        {
            repository.Update(moqCipher.Object);
        }

        [When(@"I delete a cipher")]
        public void WhenIDeleteACipher()
        {
            repository.Delete(moqCipher.Object);
        }

        [When(@"I load the ciphers")]
        public void WhenILoadTheCiphers()
        {
            repository = CipherRepository.Create();
        }

        [When(@"I SetCurrentItem")]
        public void WhenISetCurrentItem()
        {
            repository.SetCurrentItem(x => x.CipherName == "MoqCipherName");
        }

        [Then(@"the CurrentItem will be set")]
        public void ThenTheCurrentItemWillBeSet()
        {
            Assert.AreEqual("MoqCipherName", repository.CurrentItem.CipherName);
        }

        [Then(@"the CurrentItem will not be set")]
        public void ThenTheCurrentItemWillNotBeSet()
        {
            Assert.IsNull(repository.CurrentItem);
        }

        [Then(@"there should be ""(.*)"" ciphers")]
        public void ThenThereShouldBeCiphers(int p0)
        {
            Assert.AreEqual(p0, repository.Read().Count);
        }
    }
}