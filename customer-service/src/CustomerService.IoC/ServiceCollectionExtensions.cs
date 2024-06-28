using CustomerService.Application.Interfaces.Services;
using CustomerService.Domain.Interfaces.Messaging;
using CustomerService.Domain.Interfaces.Repositories;
using CustomerService.Infrastructure.Messaging;
using CustomerService.Infrastructure.Repositories;
using CustomerService.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

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

        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            configuration.GetSection("RabbitMQ").Bind(rabbitMqOptions);

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqOptions.Hostname,
                UserName = rabbitMqOptions.Username,
                Password = rabbitMqOptions.Password
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: rabbitMqOptions.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            services.AddSingleton<IModel>(channel);
            services.AddSingleton<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}
