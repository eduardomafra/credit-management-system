using CustomerService.Application.DTOs;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces.Messaging;
using CustomerService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerService.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IMessagePublisher> _messagePublisherMock;
        private readonly Mock<ILogger<Application.Services.CustomerService>> _loggerMock;
        private readonly Application.Services.CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _messagePublisherMock = new Mock<IMessagePublisher>();
            _loggerMock = new Mock<ILogger<Application.Services.CustomerService>>();
            _customerService = new Application.Services.CustomerService(
                _loggerMock.Object,
                _customerRepositoryMock.Object,
                _messagePublisherMock.Object
                );
        }

        [Fact]
        public async Task RegisterCustomer_ValidCustomer_ReturnsSuccess()
        {
            var customerDto = new CustomerDto
            {
                Name = "Teste",
                Email = "teste@teste.com",
                Document = "98358429061",
                BirthDate = DateTime.Now.AddYears(-30),
                Phone = "1234567890",
                FinancialProfile = new FinancialProfileDto()
            };

            var customer = new Customer
            {
                CustomerId = 99999,
                Name = "Teste",
                Email = "teste@teste.com",
                Document = "98358429061",
                BirthDate = DateTime.Now.AddYears(-30),
                Phone = "1234567890",
                FinancialProfile = new FinancialProfile()
            };

            _customerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
                                   .Returns(Task.CompletedTask);

            var result = await _customerService.RegisterCustomer(customerDto);

            Assert.True(result.Success);
            Assert.Equal("Cliente registrado com sucesso.", result.Result);
            _customerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
            _messagePublisherMock.Verify(pub => pub.Publish(It.IsAny<FinancialProfile>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RegisterCustomer_InvalidCustomer_ReturnsValidationErrors()
        {
            var customerDto = new CustomerDto
            {
                Name = "",
                Email = "teste@teste.com",
                Document = "98358429061",
                BirthDate = DateTime.Now.AddYears(-30),
                Phone = "1234567890",
                FinancialProfile = new FinancialProfileDto()
            };

            var result = await _customerService.RegisterCustomer(customerDto);

            Assert.False(result.Success);
            Assert.Contains("Nome é obrigatório.", result.Errors);
            Assert.Equal(400, result.StatusCode);
            _customerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Never);
            _messagePublisherMock.Verify(pub => pub.Publish(It.IsAny<FinancialProfile>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RegisterCustomer_ThrowsException_ReturnsInternalError()
        {
            var customerDto = new CustomerDto
            {
                Name = "Teste",
                Email = "teste@teste.com",
                Document = "98358429061",
                BirthDate = DateTime.Now.AddYears(-30),
                Phone = "1234567890",
                FinancialProfile = new FinancialProfileDto()
            };

            _customerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
                                   .ThrowsAsync(new Exception("Database error"));

            var result = await _customerService.RegisterCustomer(customerDto);

            Assert.False(result.Success);
            Assert.Contains("Ocorreu um erro interno", result.Errors);
            Assert.Equal(500, result.StatusCode);
            _customerRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Customer>()), Times.Once);
            _messagePublisherMock.Verify(pub => pub.Publish(It.IsAny<FinancialProfile>(), It.IsAny<string>()), Times.Never);
        }
    }
}
