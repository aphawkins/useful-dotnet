// <copyright file="ResourcesSteps.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

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