using CreditCardService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditCardService.Infrastructure.Data.Configurations
{
    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.HasKey(a => a.CreditCardId);

            builder.Property(a => a.CustomerId)
                   .IsRequired();

            builder.Property(a => a.CreditProposalId)
                   .IsRequired();

            builder.Property(a => a.CardNumber)
                   .IsRequired();

            builder.Property(a => a.CardName)
                   .IsRequired();

            builder.Property(a => a.ExpirityDate)
                   .IsRequired();

            builder.Property(a => a.CVV)
                   .IsRequired();

            builder.Property(a => a.CreditLimit)
                   .IsRequired();

            builder.Property(a => a.CreatedAt)
                   .IsRequired();
        }
    }
}
