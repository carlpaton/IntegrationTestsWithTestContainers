using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CustomerApi.Infrastructure
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer", DatabaseContext.DEFAULT_SCHEMA);

            builder.HasKey("Id");

            builder.Property<string>("Name").IsRequired();
        }
    }
}
