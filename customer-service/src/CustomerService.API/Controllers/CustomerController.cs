using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        public readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDto customerDto)
        {
            var result = await _customerService.RegisterCustomer(customerDto);

            if (result.Success)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
    }
}
