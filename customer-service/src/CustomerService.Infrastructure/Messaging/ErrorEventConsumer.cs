using CustomerService.Application.Interfaces.Services;
using CustomerService.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CustomerService.Infrastructure.Messaging
{
    public class ErrorEventConsumer : BackgroundService
    {
        private readonly ILogger<ErrorEventConsumer> _logger;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ErrorEventConsumer(ILogger<ErrorEventConsumer> logger, IModel channel, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _channel = channel;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Error event received: {Message}", message);

                var errorEvent = JsonSerializer.Deserialize<ErrorEvent>(message);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
                    customerService.ProcessErrorEvent(errorEvent);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: "error-queue", autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
