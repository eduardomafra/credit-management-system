using CreditProposalService.Application.DTOs;
using CreditProposalService.Application.Interfaces.Services;
using CreditProposalService.Application.Settings;
using CreditProposalService.Domain.Entities;
using CreditProposalService.Domain.Interfaces.Messaging;
using CreditProposalService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CreditProposalService.Application.Services
{
    public class CreditProposalService : ICreditProposalService
    {
        private readonly ILogger<CreditProposalService> _logger;
        private readonly ICreditProposalRepository _creditProposalRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly CreditProposalSettings _settings;

        public CreditProposalService(ILogger<CreditProposalService> logger,
            ICreditProposalRepository creditProposalRepository, 
            IMessagePublisher messagePublisher, 
            IOptions<CreditProposalSettings> settings)
        {
            _logger = logger;
            _creditProposalRepository = creditProposalRepository;
            _messagePublisher = messagePublisher;
            _settings = settings.Value;
        }

        public async Task ProcessProposal(FinancialProfileDto financialProfile)
        {
            try
            {
                _logger.LogInformation($"Processing credit proposal for CustomerId: {financialProfile.CustomerId}");

                var creditProposal = new CreditProposal(financialProfile.CustomerId, financialProfile.FinancialProfileId, CalculateCreditLimit(financialProfile));
                await _creditProposalRepository.AddAsync(creditProposal);

                _logger.LogInformation($"Credit proposal for CustomerId: {financialProfile.CustomerId} added successfully");

                _messagePublisher.Publish(creditProposal, "credit-proposal-queue");
                _logger.LogInformation($"Credit proposal for CustomerId: {financialProfile.CustomerId} published successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing credit proposal for CustomerId: {financialProfile.CustomerId}");
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
            _logger.LogInformation($"Calculated credit limit for CustomerId: {profile.CustomerId} is {creditLimit}");
            return creditLimit;
        }
    }
}
