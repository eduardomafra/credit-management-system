using CreditCardService.Application.DTOs;

namespace CreditCardService.Application.Interfaces.Services
{
    public interface ICreditCardService
    {
        Task ProcessCreditCard(CreditProposalDto creditProposal);
    }
}
