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
            var rabbitMqOptions = new RabbitMqSettings();
            configuration.GetSection("RabbitMQ").Bind(rabbitMqOptions);

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqOptions.Hostname,
                UserName = rabbitMqOptions.Username,
                Port = rabbitMqOptions.Port,
                Password = rabbitMqOptions.Password
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "customer-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.ExchangeDeclare(exchange: "dead-letter-exchange", type: "direct");
            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dead-letter-exchange" },
                { "x-dead-letter-routing-key", "dead-letter-routing-key" }
            };

            channel.QueueDeclare(queue: "dead-letter-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "dead-letter-queue", exchange: "dead-letter-exchange", routingKey: "dead-letter-routing-key");

            channel.ExchangeDeclare(exchange: "error-exchange", type: "direct");
            channel.QueueDeclare(queue: "error-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "error-queue", exchange: "error-exchange", routingKey: "error-routing-key");

            services.AddSingleton<IModel>(channel);
            services.AddSingleton<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}
