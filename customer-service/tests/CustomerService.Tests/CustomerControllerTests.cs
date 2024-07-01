using CustomerService.API.Controllers;
using CustomerService.Application.DTOs;
using CustomerService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CustomerService.Tests
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomerController _customerController;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _customerController = new CustomerController(_customerServiceMock.Object);
        }

        [Fact]
        public async Task Post_ValidCustomer_ReturnsOk()
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

            var response = new ApiResponse<string>("Cliente registrado com sucesso.");
            _customerServiceMock.Setup(service => service.RegisterCustomer(customerDto))
                                .ReturnsAsync(response);

            var result = await _customerController.Post(customerDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResponse = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(returnedResponse.Success);
            Assert.Equal("Cliente registrado com sucesso.", returnedResponse.Result);
        }

        [Fact]
        public async Task Post_InvalidCustomer_ReturnsBadRequest()
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

            var response = new ApiResponse<string>(new List<string> { "Nome é obrigatório." }, 400);
            _customerServiceMock.Setup(service => service.RegisterCustomer(customerDto))
                                .ReturnsAsync(response);

            var result = await _customerController.Post(customerDto);

            var badRequestResult = Assert.IsType<ObjectResult>(result);
            var returnedResponse = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(returnedResponse.Success);
            Assert.Contains("Nome é obrigatório.", returnedResponse.Errors);
            Assert.Equal(400, returnedResponse.StatusCode);
        }
    }
}
