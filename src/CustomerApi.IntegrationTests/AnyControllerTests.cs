using CustomerApi.Infrastructure;
using CustomerApi.IntegrationTests.Setup;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests;

public class AnyControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _client;
    private readonly CustomerApiFactory _factory;

    public AnyControllerTests(CustomerApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Ping_ReturnsOk_New()
    {
        // Act
        using var response = await _client.GetAsync("ping");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_CreatesUser_WhenDataIsValid_New()
    {
        // Arrange
        var customer = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Le Names " + DateTime.Now
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "customer")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(customer),
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act
        using var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
