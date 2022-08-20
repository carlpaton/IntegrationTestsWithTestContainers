using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;

namespace CustomerApi.IntegrationTests.Setup
{
    public class TestBase : IClassFixture<CustomerApiFactory>
    {
        protected HttpClient HttpClient { get; }

        public TestBase(CustomerApiFactory factory)
        {
            HttpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
    }
}
