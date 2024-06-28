using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces.Services;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces.Repositories;
using Mapster;

namespace CustomerService.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ApiResponse<bool>> RegisterCustomer(CustomerDto dto)
        {
            try
            {
                var customer = dto.Adapt<Customer>();
                await _customerRepository.AddAsync(customer);

                return new ApiResponse<bool>(true);
            }
            catch (Exception)
            {
                return new ApiResponse<bool>(new List<string> { "Ocorreu um erro interno" });
            }
        }
    }
}
