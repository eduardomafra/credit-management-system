using CreditProposalService.Application.DTOs;

namespace CreditProposalService.Application.Interfaces.Services
{
    public interface ICreditProposalService
    {
        Task ProcessProposal(FinancialProfileDto financialProfile);
    }
}
