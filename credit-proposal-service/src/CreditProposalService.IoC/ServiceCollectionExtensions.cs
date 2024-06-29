using CreditProposalService.Application.Interfaces.Services;
using CreditProposalService.Application.Settings;
using CreditProposalService.Domain.Interfaces.Messaging;
using CreditProposalService.Domain.Interfaces.Repositories;
using CreditProposalService.Infrastructure.Messaging;
using CreditProposalService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace CreditProposalService.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICreditProposalRepository, CreditProposalRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICreditProposalService, Application.Services.CreditProposalService>();

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
                Password = rabbitMqOptions.Password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: rabbitMqOptions.CreditProposalQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            services.AddSingleton<IModel>(channel);
            services.AddSingleton<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}
