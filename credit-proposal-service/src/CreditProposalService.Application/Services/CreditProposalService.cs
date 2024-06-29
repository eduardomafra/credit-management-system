using CreditProposalService.Application.DTOs;
using CreditProposalService.Application.Interfaces.Services;
using CreditProposalService.Application.Settings;
using CreditProposalService.Domain.Entities;
using CreditProposalService.Domain.Interfaces.Messaging;
using CreditProposalService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;

namespace CreditProposalService.Application.Services
{
    public class CreditProposalService : ICreditProposalService
    {
        private readonly ICreditProposalRepository _creditProposalRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly CreditProposalSettings _settings;

        public CreditProposalService(ICreditProposalRepository creditProposalRepository, 
            IMessagePublisher messagePublisher, 
            IOptions<CreditProposalSettings> settings)
        {
            _creditProposalRepository = creditProposalRepository;
            _messagePublisher = messagePublisher;
            _settings = settings.Value;
        }

        public async Task ProcessProposal(FinancialProfileDto financialProfile)
        {
            try
            {
                var creditProposal = new CreditProposal(financialProfile.CustomerId, financialProfile.FinancialProfileId, CalculateCreditLimit(financialProfile));
                await _creditProposalRepository.AddAsync(creditProposal);

                _messagePublisher.Publish(creditProposal);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private decimal CalculateCreditLimit(FinancialProfileDto profile)
        {
            decimal baseLimit = profile.MonthlyIncome * _settings.BasePercentage;
            decimal creditScoreFactor = (profile.CreditScore / _settings.ScoreDivider) * profile.MonthlyIncome;
            decimal homeOwnershipFactor = profile.OwnsHome ? profile.MonthlyIncome * _settings.PropertyPercentage : 0;
            decimal vehicleOwnershipFactor = profile.OwnsVehicle ? profile.MonthlyIncome * _settings.VehiclePercentage : 0;

            decimal creditLimit = baseLimit + creditScoreFactor + homeOwnershipFactor + vehicleOwnershipFactor;
            return creditLimit;
        }
    }
}
