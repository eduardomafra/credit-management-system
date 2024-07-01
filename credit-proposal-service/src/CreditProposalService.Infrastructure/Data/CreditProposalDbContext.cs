using CreditProposalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditProposalService.Infrastructure.Data
{
    public class CreditProposalDbContext : DbContext
    {
        public CreditProposalDbContext(DbContextOptions<CreditProposalDbContext> options) : base(options) { }

        public DbSet<CreditProposal> CreditProposals { get; set; }
    }
}
