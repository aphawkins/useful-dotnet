// <copyright file="EndpointTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace WebApp.Tests
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    [Collection("Sequential")]
    public class EndpointTests : IClassFixture<WebApplicationFactory<UsefulWeb.Startup>>
    {
        private readonly WebApplicationFactory<UsefulWeb.Startup> _factory;

        public EndpointTests(WebApplicationFactory<UsefulWeb.Startup> factory) => _factory = factory;

        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        [InlineData("/Cryptography")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}