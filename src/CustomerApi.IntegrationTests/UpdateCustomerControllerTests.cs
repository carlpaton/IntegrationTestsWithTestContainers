using CustomerApi.Infrastructure;
using CustomerApi.IntegrationTests.Helpers;
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

public class UpdateCustomerControllerTests : TestBase
{
    private readonly CustomerApiHelpers _helper;

    public UpdateCustomerControllerTests(CustomerApiFactory factory) : base(factory)
    {
        _helper = new CustomerApiHelpers(HttpClient);
    }

    [Fact]
    public async Task Update_UpdatesTheGivenCustomer_WhenParametersAreValid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customerName = "new customer";
        await _helper.CreateCustomerAsync(customerId, customerName);

        var customerUpdatedName = "updated customer name";
        var customerUpdate = new Customer()
        {
            Name = customerUpdatedName
        };

        var request = new HttpRequestMessage(HttpMethod.Put, $"customer/{customerId}")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(customerUpdate),
                Encoding.UTF8,
                MediaTypeNames.Application.Json)
        };

        // Act
        using var response = await HttpClient.SendAsync(request);
        var stream = await response.Content.ReadAsStreamAsync();
        var customer = JsonSerializer.Deserialize<Customer>(stream, SerializerOptions.Default);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        customer.Id.Should().Be(customerId);
        customer.Name.Should().Be(customerUpdatedName);
    }
}
