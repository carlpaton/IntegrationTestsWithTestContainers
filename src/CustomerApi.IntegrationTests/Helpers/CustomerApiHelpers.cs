using CustomerApi.Infrastructure;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerApi.IntegrationTests.Helpers;

public class CustomerApiHelpers
{
    private readonly HttpClient _httpClient;

    public CustomerApiHelpers(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateCustomerAsync(string name, bool appendDateToName = true) 
    {
        var customer = new Customer()
        {
            Id = Guid.NewGuid(),
            Name = GetCustomerName(name, appendDateToName)
        };
        await PostCustomer(customer);
    }

    public async Task CreateCustomerAsync(Guid id, string name, bool appendDateToName = true)
    {
        var customer = new Customer()
        {
            Id = id,
            Name = GetCustomerName(name, appendDateToName)
        };
        await PostCustomer(customer);
    }

    private string GetCustomerName(string name, bool appendDateToName) 
    {
        var nameForUpdate = name;
        if (appendDateToName)
        {
            nameForUpdate = $"{name} {DateTime.Now}";
        }

        return nameForUpdate;
    }

    private async Task PostCustomer(Customer customer) 
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "customer")
        {
            Content = new StringContent(
               JsonSerializer.Serialize(customer),
               Encoding.UTF8,
               MediaTypeNames.Application.Json)
        };
        await _httpClient.SendAsync(request);
    }
}
