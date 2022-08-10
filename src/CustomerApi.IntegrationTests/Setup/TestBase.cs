using CustomerApi.IntegarationTests.Setup;
using System;
using System.Net.Http;

namespace CustomerApi.IntegrationTests.Setup;

public class TestBase : IDisposable
{
    protected HttpClient HttpClient { get; }

    protected TestBase()
    {
        var server = CustomerApiFactory.GetWebApplicationFactory();
        HttpClient = server.CreateClient();
    }

    public void Dispose() => HttpClient.Dispose();
}
