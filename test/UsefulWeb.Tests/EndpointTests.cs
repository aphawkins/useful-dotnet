// Copyright (c) Andrew Hawkins. All rights reserved.

using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace UsefulWeb.Tests
{
    [Collection("Sequential")]
    public class EndpointTests(WebApplicationFactory<Startup> factory) : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory = factory;

        [Theory]
        [InlineData("/")]
        [InlineData("/Home")]
        [InlineData("/Cryptography")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentTypeAsync(string url)
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
