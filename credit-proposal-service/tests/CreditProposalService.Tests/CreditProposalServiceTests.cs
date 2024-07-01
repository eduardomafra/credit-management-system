using CreditProposalService.Application.DTOs;
using CreditProposalService.Application.Settings;
using CreditProposalService.Domain.Entities;
using CreditProposalService.Domain.Interfaces.Messaging;
using CreditProposalService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CreditProposalService.Tests
{
    public class CreditProposalServiceTests
    {
        private readonly Mock<ICreditProposalRepository> _creditProposalRepositoryMock;
        private readonly Mock<IMessagePublisher> _messagePublisherMock;
        private readonly Mock<IOptions<CreditProposalSettings>> _settingsMock;
        private readonly Mock<ILogger<Application.Services.CreditProposalService>> _loggerMock;
        private readonly Application.Services.CreditProposalService _creditProposalService;

        public CreditProposalServiceTests()
        {
            _creditProposalRepositoryMock = new Mock<ICreditProposalRepository>();
            _messagePublisherMock = new Mock<IMessagePublisher>();
            _settingsMock = new Mock<IOptions<CreditProposalSettings>>();
            _loggerMock = new Mock<ILogger<Application.Services.CreditProposalService>>();

            var settings = new CreditProposalSettings
            {
                BasePercentage = 0.3m,
                ScoreDivider = 1000m,
                PropertyPercentage = 0.2m,
                VehiclePercentage = 0.1m
            };

            _settingsMock.Setup(s => s.Value).Returns(settings);

            _creditProposalService = new Application.Services.CreditProposalService(
                _loggerMock.Object,
                _creditProposalRepositoryMock.Object,
                _messagePublisherMock.Object,
                _settingsMock.Object
            );
        }

        [Fact]
        public async Task ProcessProposal_ValidFinancialProfile_AddsProposalAndPublishesMessage()
        {
            var financialProfile = new FinancialProfileDto
            {
                CustomerId = 99999,
                FinancialProfileId = 99999,
                MonthlyIncome = 5000m,
                CreditScore = 750,
                OwnsHome = true,
                OwnsVehicle = true
            };

            await _creditProposalService.ProcessProposal(financialProfile);

            _creditProposalRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CreditProposal>()), Times.Once);
            _messagePublisherMock.Verify(pub => pub.Publish(It.IsAny<CreditProposal>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ProcessProposal_ThrowsException_PropagatesException()
        {
            var financialProfile = new FinancialProfileDto
            {
                CustomerId = 99999,
                FinancialProfileId = 99999,
                MonthlyIncome = 5000m,
                CreditScore = 750,
                OwnsHome = true,
                OwnsVehicle = true
            };

            _creditProposalRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CreditProposal>()))
                                         .ThrowsAsync(new Exception("Database error"));

            var exception = await Assert.ThrowsAsync<Exception>(() => _creditProposalService.ProcessProposal(financialProfile));
            Assert.Equal("Database error", exception.Message);

            _creditProposalRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CreditProposal>()), Times.Once);
            _messagePublisherMock.Verify(pub => pub.Publish(It.IsAny<CreditProposal>(), It.IsAny<string>()), Times.Never);
        }
    }
}