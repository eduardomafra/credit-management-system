﻿using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces.Services;
using CustomerService.Application.Validators;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces.Messaging;
using CustomerService.Domain.Interfaces.Repositories;
using CustomerService.Domain.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CustomerService.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessagePublisher _messagePublisher;

        public CustomerService(ILogger<CustomerService> logger, ICustomerRepository customerRepository, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _messagePublisher = messagePublisher;
        }

        public async Task<ApiResponse<string>> RegisterCustomer(CustomerDto dto)
        {
            try
            {
                _logger.LogInformation($"Starting customer registration for {dto.Name} with email {dto.Email}");

                var validator = new CustomerDtoValidator();
                var validationResult = await validator.ValidateAsync(dto);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return new ApiResponse<string>(errorMessages, (int)System.Net.HttpStatusCode.BadRequest);
                }

                var customer = dto.Adapt<Customer>();
                await _customerRepository.AddAsync(customer);
                _logger.LogInformation($"Customer {customer.CustomerId} added successfully to the database");

                _messagePublisher.Publish(customer.FinancialProfile, "customer-queue");
                _logger.LogInformation($"Financial profile published for customer {customer.CustomerId}");

                return new ApiResponse<string>("Cliente registrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while registering customer with name {dto.Name} email {dto.Email}");
                return new ApiResponse<string>(new List<string> { "Ocorreu um erro interno" });
            }
        }
        public void ProcessErrorEvent(ErrorEvent errorEvent) =>     
            _logger.LogError($"Error received from microservice: {errorEvent.Microservice}. Error message: {errorEvent.ErrorMessage}. Original message: {errorEvent.OriginalMessage}. Timestamp: {errorEvent.Timestamp}");
        
    }
}
