using CustomerApi.Infrastructure;
using CustomerApi.IntegrationTests.Helpers;
using CustomerApi.IntegrationTests.Setup;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests;

public class GetAllCustomerControllerTests : TestBase
{
    private readonly CustomerApiHelpers _helper;

    public GetAllCustomerControllerTests(CustomerApiFactory factory) : base(factory) 
    {
        _helper = new CustomerApiHelpers(HttpClient);
    }

    [Fact]
    public async Task GetAll_ReturnsAllCustomers_WhenTheyExist()
    {
        // Arrange
        var noCustomersToCreate = 10;
        var tasks = new List<Task>();
        for (int i = 0; i < noCustomersToCreate; i++)
        {
            tasks.Add(_helper.CreateCustomerAsync("new customer"));
        }
        await Task.WhenAll(tasks);

        var request = new HttpRequestMessage(HttpMethod.Get, "customer");

        // Act
        using var response = await HttpClient.SendAsync(request);
        var stream = await response.Content.ReadAsStreamAsync();
        var customers = JsonSerializer.Deserialize<List<Customer>>(stream, SerializerOptions.Default);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        customers.Count.Should().Be(noCustomersToCreate);
    }
}
