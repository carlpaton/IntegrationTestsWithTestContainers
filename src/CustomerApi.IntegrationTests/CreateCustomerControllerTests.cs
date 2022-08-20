using CustomerApi.Controllers;
using Xunit;
using Moq;
using CustomerApi.Infrastructure;
using System;
using FluentAssertions;
using System.Threading.Tasks;
using System.Net;
using CustomerApi.IntegrationTests.Setup;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace CustomerApi.IntegrationTests;

public class CreateCustomerControllerTests : OldTestBase
{
    [Fact]
    public async Task Create_CreatesUser_WhenDataIsValid()
    {
        // Arrange
        var customer = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Le Names"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "customer")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(customer),
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act
        using var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ReturnsValidationError_WhenDataIsInvalid()
    {
        // Arrange
        var customer = new Customer()
        {
            Id = Guid.Empty
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "customer")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(customer),
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act
        using var response = await HttpClient.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
