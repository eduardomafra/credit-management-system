﻿using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.Infrastructure.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.AddressId);

            builder.Property(a => a.Street)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(a => a.Number)
                   .IsRequired(false)
                   .HasMaxLength(15);

            builder.Property(a => a.Complement)
                   .IsRequired(false)
                   .HasMaxLength(30);

            builder.Property(a => a.Neighborhood)
                   .IsRequired()
                   .HasMaxLength(30);

            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(a => a.State)
                   .IsRequired()
                   .HasMaxLength(2);

            builder.Property(a => a.ZipCode)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.HasOne<Customer>()
               .WithOne()
               .HasForeignKey<FinancialProfile>("CustomerId");
        }
    }
}
