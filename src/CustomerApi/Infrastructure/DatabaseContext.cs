using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Infrastructure;

public class DatabaseContext : DbContext
{
    public const string DEFAULT_SCHEMA = "dbo";
    public DbSet<Customer> Customers { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options) { }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
    //    base.OnModelCreating(modelBuilder);
    //}
}
