using CreditProposalService.Application.DTOs;
using CreditProposalService.Application.Interfaces.Services;
using CreditProposalService.Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CreditProposalService.Infrastructure.Messaging
{
    public class MessageConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMqSettings _options;

        public MessageConsumer(IModel channel, IServiceScopeFactory serviceScopeFactory, IOptions<RabbitMqSettings> options)
        {
            _channel = channel;
            _serviceScopeFactory = serviceScopeFactory;
            _options = options.Value;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message received: {message}");

                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var creditProposalService = scope.ServiceProvider.GetRequiredService<ICreditProposalService>();
                        var financialProfile = JsonSerializer.Deserialize<FinancialProfileDto>(message);
                        await creditProposalService.ProcessProposal(financialProfile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: _options.CustomerQueue, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
