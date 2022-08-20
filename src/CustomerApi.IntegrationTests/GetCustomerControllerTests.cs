using CustomerApi.Infrastructure;
using CustomerApi.IntegrationTests.Helpers;
using CustomerApi.IntegrationTests.Setup;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests;

public class GetCustomerControllerTests : TestBase
{
    private readonly CustomerApiHelpers _helper;

    public GetCustomerControllerTests(CustomerApiFactory factory) : base(factory) 
    {
        _helper = new CustomerApiHelpers(HttpClient);
    }

    [Fact]
    public async Task Read_ReturnsCustomerById_WhenItExists()
    {
        // Arrange
        var appendDateToName = false;
        var customerId = Guid.NewGuid();
        var customerName = "new customer";
        await _helper.CreateCustomerAsync(customerId, customerName, appendDateToName);

        var request = new HttpRequestMessage(HttpMethod.Get, $"customer/{customerId}");

        // Act
        using var response = await HttpClient.SendAsync(request);
        var stream = await response.Content.ReadAsStreamAsync();
        var customer = JsonSerializer.Deserialize<Customer>(stream, SerializerOptions.Default);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        customer.Id.Should().Be(customerId);
        customer.Name.Should().Be(customerName);
    }
}
