using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace CustomerApi.IntegrationTests
{
    public class TestServer
    {
        private static WebApplicationFactory<Program> _server;

        public static WebApplicationFactory<Program> Get()
        {
            if (_server != null)
                return _server;

            return _server =
                new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                        builder
                            .UseEnvironment("test")
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.testserver.json")));
        }

        public void Dispose()
        {
            _server.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
