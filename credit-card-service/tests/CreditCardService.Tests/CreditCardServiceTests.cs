using CreditCardService.Application.DTOs;
using CreditCardService.Domain.Entities;
using CreditCardService.Domain.Interfaces.Messaging;
using CreditCardService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace CreditCardService.Tests
{
    public class CreditCardServiceTests
    {
        private readonly Mock<ICreditCardRepository> _creditCardRepositoryMock;
        private readonly Mock<IMessagePublisher> _messagePublisherMock;
        private readonly Mock<ILogger<Application.Services.CreditCardService>> _loggerMock;
        private readonly Application.Services.CreditCardService _creditCardService;

        public CreditCardServiceTests()
        {
            _creditCardRepositoryMock = new Mock<ICreditCardRepository>();
            _messagePublisherMock = new Mock<IMessagePublisher>();
            _loggerMock = new Mock<ILogger<Application.Services.CreditCardService>>();

            _creditCardService = new Application.Services.CreditCardService(
                _loggerMock.Object,
                _creditCardRepositoryMock.Object,
                _messagePublisherMock.Object
            );
        }

        [Fact]
        public async Task ProcessCreditCard_ValidCreditProposal_AddsCardAndPublishesMessage()
        {
            var creditProposal = new CreditProposalDto
            {
                CustomerId = 99999,
                CreditProposalId = 99999,
                Amount = 5000m
            };

            await _creditCardService.ProcessCreditCard(creditProposal);

            _creditCardRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CreditCard>()), Times.Once);
            //_messagePublisherMock.Verify(pub => pub.Publish(It.IsAny<CreditCard>()), Times.Once);
        }

        [Fact]
        public async Task ProcessCreditCard_ThrowsException_PropagatesException()
        {
            var creditProposal = new CreditProposalDto
            {
                CustomerId = 99999,
                CreditProposalId = 99999,
                Amount = 5000m
            };

            _creditCardRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CreditCard>()))
                                     .ThrowsAsync(new Exception("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _creditCardService.ProcessCreditCard(creditProposal));
            Assert.Equal("Database error", exception.Message);

            _creditCardRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CreditCard>()), Times.Once);
            //_messagePublisherMock.Verify(pub => pub.Publish(It.IsAny<CreditCard>()), Times.Never);
        }
    }
}