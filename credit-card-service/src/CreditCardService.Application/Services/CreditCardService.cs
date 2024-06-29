using CreditCardService.Application.DTOs;
using CreditCardService.Application.Interfaces.Services;
using CreditCardService.Domain.Entities;
using CreditCardService.Domain.Interfaces.Messaging;
using CreditCardService.Domain.Interfaces.Repositories;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CreditCardService.Application.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IMessagePublisher _messagePublisher;

        public CreditCardService(ICreditCardRepository creditCardRepository,
            IMessagePublisher messagePublisher)
        {
            _creditCardRepository = creditCardRepository;
            _messagePublisher = messagePublisher;
        }

        public async Task ProcessCreditCard(CreditProposalDto creditProposal)
        {
            try
            {
                var creditCard = new CreditCard(creditProposal.CustomerId, creditProposal.CreditProposalId, GenerateCardNumber(),
                    GenerateCVV(), string.Empty, creditProposal.Amount);
                await _creditCardRepository.AddAsync(creditCard);

                _messagePublisher.Publish(creditCard);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string GenerateCardNumber()
        {
            Random random = new Random();
            StringBuilder cardNumber = new StringBuilder();

            for (int i = 0; i < 16; i++)
                cardNumber.Append(random.Next(0, 10));

            return cardNumber.ToString();
        }

        private string GenerateCVV()
        {
            Random random = new Random();
            StringBuilder cvv = new StringBuilder();

            for (int i = 0; i < 3; i++)
                cvv.Append(random.Next(0, 10));

            return cvv.ToString();
        }
    }
}
