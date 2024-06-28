using CustomerService.Application.DTOs;

namespace CustomerService.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<ApiResponse<bool>> RegisterCustomer(CustomerDto dto);
    }
}
