using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(c => c.Document)
                   .IsRequired()
                   .HasMaxLength(14);

            builder.Property(c => c.BirthDate)
                   .IsRequired();

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Phone)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.RegistrationDate)
                   .IsRequired();

            builder.HasOne(c => c.FinancialProfile)
                   .WithOne()
                   .HasForeignKey<FinancialProfile>(f => f.CustomerId);
        }
    }
}
