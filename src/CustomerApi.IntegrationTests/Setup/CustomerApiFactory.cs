using CustomerApi.Infrastructure;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests.Setup;

public class CustomerApiFactory : 
    WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer _dbContainer;

    public CustomerApiFactory()
    {
        _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "mydb",
                Username = "postgres",
                Password = "password",
            })
            .Build();
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var maxRetryCount = 5;
        var maxRetryDelay = 30;

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<DatabaseContext>));

            services.Remove(descriptor);
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
        });
    }
}
