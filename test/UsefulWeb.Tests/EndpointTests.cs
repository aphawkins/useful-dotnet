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
        public async Task GetEndpointsReturnSuccessAndCorrectContentTypeAsync(string path)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(new Uri(path, UriKind.Relative)).ConfigureAwait(true);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }
    }
}
