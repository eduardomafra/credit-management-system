using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Infrastructure.Data.Configurations
{
    public class FinancialProfileConfiguration : IEntityTypeConfiguration<FinancialProfile>
    {
        public void Configure(EntityTypeBuilder<FinancialProfile> builder)
        {
            builder.HasKey(f => f.FinancialProfileId);

            builder.Property(f => f.MonthlyIncome)
                   .IsRequired();

            builder.Property(f => f.CreditScore)
                   .IsRequired();

            builder.Property(f => f.OwnsHome)
                   .IsRequired();

            builder.Property(f => f.OwnsVehicle)
                   .IsRequired();
                        
            builder.HasOne<Customer>()
               .WithOne()
               .HasForeignKey<FinancialProfile>("CustomerId");
        }
    }
}
