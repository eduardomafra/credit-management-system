using CreditCardService.Application.Interfaces.Services;
using CreditCardService.Application.Settings;
using CreditCardService.Domain.Interfaces.Messaging;
using CreditCardService.Domain.Interfaces.Repositories;
using CreditCardService.Infrastructure.Messaging;
using CreditCardService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CreditCardService.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICreditCardService, Application.Services.CreditCardService>();

            return services;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqOptions = new RabbitMqSettings();
            configuration.GetSection("RabbitMQ").Bind(rabbitMqOptions);

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqOptions.Hostname,
                Port = rabbitMqOptions.Port,
                UserName = rabbitMqOptions.Username,
                Password = rabbitMqOptions.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            services.AddSingleton<IModel>(channel);
            services.AddSingleton<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}
