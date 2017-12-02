namespace Useful.UI.UnitTests.Resources
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;
    using UI.Resources;

    [Binding]
    public class ResourcesSteps
    {
        [Given(@"I have resources")]
        public void GivenIHaveResources()
        {
        }

        [Then(@"I can get the Application Icon")]
        public void ThenICanGetTheApplicationIcon()
        {
            Assert.IsNotNull(Resources.GetAppIcon());
        }
    }
}