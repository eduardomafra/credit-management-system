using CreditProposalService.Domain.Entities;
using CreditProposalService.Domain.Interfaces.Repositories;
using CreditProposalService.Infrastructure.Data;

namespace CreditProposalService.Infrastructure.Repositories
{
    public class CreditProposalRepository : BaseRepository<CreditProposal>, ICreditProposalRepository
    {
        public CreditProposalRepository(CreditProposalDbContext context) : base(context)
        {
        }
    }
}
