using CustomerApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:CustomerApiDatabase");
var maxRetryCount = builder.Configuration.GetValue<int>("PostgreSQL:MaxRetryCount");
var maxRetryDelay = builder.Configuration.GetValue<int>("PostgreSQL:MaxRetryDelay");
var errorCodesToAdd = builder.Configuration.GetValue<ICollection<string>>("PostgreSQL:ErrorCodesToAdd");

builder.Services.AddDbContext<DatabaseContext>((serviceProvider, optionsBuilder) =>
    {
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                    maxRetryCount, 
                    TimeSpan.FromSeconds(maxRetryDelay), 
                    errorCodesToAdd);
            });
    }
);

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// This is pretty trash as the `CustomerApi` is now polluted with Intergration tests concearns
if (app.Environment.IsEnvironment("Test"))
{
    // Apply migrations at runtime
    // Need to first run `dotnet ef migrations add MeaningfulMigrationName`
    // https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program
{
}