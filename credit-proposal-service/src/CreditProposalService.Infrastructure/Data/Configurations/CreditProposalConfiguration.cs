using CreditProposalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Net;

namespace CreditProposalService.Infrastructure.Data.Configurations
{
    public class CreditProposalConfiguration : IEntityTypeConfiguration<CreditProposal>
    {
        public void Configure(EntityTypeBuilder<CreditProposal> builder)
        {
            builder.HasKey(a => a.CreditProposalId);

            builder.Property(a => a.CustomerId)
                   .IsRequired();

            builder.Property(a => a.Amount)
                   .IsRequired();

            builder.Property(a => a.CreatedAt)
                   .IsRequired();
        }
    }
}
