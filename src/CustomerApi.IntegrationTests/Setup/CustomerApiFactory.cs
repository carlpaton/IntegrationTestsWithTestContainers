using CustomerApi.Infrastructure;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CustomerApi.IntegrationTests.Setup;

public class CustomerApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlTestcontainer _dbContainer;
    private readonly string _appSettingsFile = "appsettings.Test.json";
    private readonly IConfiguration _configuration;

    public CustomerApiFactory()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile(_appSettingsFile)
            .Build();

        _dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = _configuration.GetSection("PostgreSQL:Database").Get<string>(),
                Username = _configuration.GetSection("PostgreSQL:Username").Get<string>(),
                Password = _configuration.GetSection("PostgreSQL:Password").Get<string>(),
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
        var maxRetryCount = _configuration.GetSection("PostgreSQL:MaxRetryCount").Get<int>();
        var maxRetryDelay = _configuration.GetSection("PostgreSQL:MaxRetryDelay").Get<int>();

        builder
        .UseEnvironment("Test")
        .UseContentRoot(Directory.GetCurrentDirectory())
        .ConfigureAppConfiguration(config => config.AddJsonFile(_appSettingsFile))
        .ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

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
