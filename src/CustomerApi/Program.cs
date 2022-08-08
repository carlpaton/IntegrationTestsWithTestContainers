using CustomerApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var postgresSettings = new PostgresSettings();
//builder.Services.AddSingleton<IPostgresSettings>(postgresSettings);

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
// _dbContext.Database.Migrate();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
