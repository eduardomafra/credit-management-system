using CustomerService.Application.DTOs;
using CustomerService.Domain.Models;

namespace CustomerService.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        void ProcessErrorEvent(ErrorEvent errorEvent);
        Task<ApiResponse<bool>> RegisterCustomer(CustomerDto dto);
    }
}
