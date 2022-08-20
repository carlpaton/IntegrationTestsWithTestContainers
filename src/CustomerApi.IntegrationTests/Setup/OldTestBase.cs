using CustomerApi.IntegarationTests.Setup;
using System;
using System.Net.Http;

namespace CustomerApi.IntegrationTests.Setup;

public class OldTestBase : IDisposable
{
    protected HttpClient HttpClient { get; }

    protected OldTestBase()
    {
        var server = OldCustomerApiFactory.GetWebApplicationFactory();
        HttpClient = server.CreateClient();
    }

    public void Dispose() => HttpClient.Dispose();
}
