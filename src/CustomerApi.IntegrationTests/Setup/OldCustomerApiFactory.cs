using CustomerApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace CustomerApi.IntegarationTests.Setup;

public class OldCustomerApiFactory : IAsyncLifetime
{
    private static readonly PostgreSqlTestcontainer _dbContainer =
        new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "mydb",
                Username = "postgres",
                Password = "password",
                Port = 5554
            })
            .Build();

    private static WebApplicationFactory<Program> _server;

    public static WebApplicationFactory<Program> GetWebApplicationFactory()
    {
        if (_server != null)
            return _server;

        var maxRetryCount = 5;
        var maxRetryDelay = 30;

        return _server =
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                    builder
                        .ConfigureTestServices(services => 
                        {
                            services.RemoveAll(typeof(DatabaseContext));
                            services.AddDbContext<DatabaseContext>((serviceProvider, optionsBuilder) =>
                            {
                                optionsBuilder.UseNpgsql(
                                    _dbContainer.ConnectionString,
                                    npgsqlOptionsAction =>
                                    {
                                        npgsqlOptionsAction.EnableRetryOnFailure(
                                            maxRetryCount,
                                            TimeSpan.FromSeconds(maxRetryDelay),
                                            null);
                                    });
                            });
                        }));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async new Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
