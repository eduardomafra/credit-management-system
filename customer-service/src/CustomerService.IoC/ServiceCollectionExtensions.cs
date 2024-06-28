using CustomerService.Application.Interfaces.Services;
using CustomerService.Domain.Interfaces.Repositories;
using CustomerService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, Application.Services.CustomerService>();

            return services;
        }
    }
}
