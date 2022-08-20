using CustomerApi.Infrastructure;
using CustomerApi.IntegrationTests.Setup;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests;

public class DeleteCustomerControllerTests : TestBase
{
    public DeleteCustomerControllerTests(CustomerApiFactory factory) : base(factory) { }

    [Fact]
    public async Task Delete_DeletesUserByCustomerId_WhenCustomerExists()
    {
        // Arrange
        var customer = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = "Delete me"
        };

        var createRequest = new HttpRequestMessage(HttpMethod.Post, "customer")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(customer),
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };
        await HttpClient.SendAsync(createRequest);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"customer/{customer.Id}")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(customer),
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act
        using var deleteResponse = await HttpClient.SendAsync(deleteRequest);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, $"customer/{customer.Id}");
        using var getResponse = await HttpClient.SendAsync(getRequest);

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
