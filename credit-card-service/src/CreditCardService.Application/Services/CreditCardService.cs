using CreditCardService.Application.DTOs;
using CreditCardService.Application.Interfaces.Services;
using CreditCardService.Domain.Entities;
using CreditCardService.Domain.Interfaces.Messaging;
using CreditCardService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CreditCardService.Application.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ILogger<CreditCardService> _logger;
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IMessagePublisher _messagePublisher;

        public CreditCardService(ILogger<CreditCardService> logger,
            ICreditCardRepository creditCardRepository,
            IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _creditCardRepository = creditCardRepository;
            _messagePublisher = messagePublisher;
        }

        public async Task ProcessCreditCard(CreditProposalDto creditProposal)
        {
            try
            {
                _logger.LogInformation($"Processing credit card for CustomerId: {creditProposal.CustomerId}");
                var creditCard = new CreditCard(creditProposal.CustomerId, creditProposal.CreditProposalId, GenerateCardNumber(),
                    GenerateCVV(), string.Empty, creditProposal.Amount);
                await _creditCardRepository.AddAsync(creditCard);

                _logger.LogInformation($"Credit card for CustomerId: {creditProposal.CustomerId} added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing credit card for CustomerId: {creditProposal.CustomerId}");
                throw;
            }
        }

        private string GenerateCardNumber()
        {
            Random random = new Random();
            StringBuilder cardNumber = new StringBuilder();

            for (int i = 0; i < 16; i++)
                cardNumber.Append(random.Next(0, 10));

            _logger.LogInformation($"Generated card number: {cardNumber}");
            return cardNumber.ToString();
        }

        private string GenerateCVV()
        {
            Random random = new Random();
            StringBuilder cvv = new StringBuilder();

            for (int i = 0; i < 3; i++)
                cvv.Append(random.Next(0, 10));

            _logger.LogInformation($"Generated CVV: {cvv}");
            return cvv.ToString();
        }
    }
}
