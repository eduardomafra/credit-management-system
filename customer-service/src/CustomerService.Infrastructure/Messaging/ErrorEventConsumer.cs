using CustomerService.Application.Interfaces.Services;
using CustomerService.Domain.Models;
using CustomerService.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CustomerService.Infrastructure.Messaging
{
    public class ErrorEventConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RabbitMqSettings _options;

        public ErrorEventConsumer(IModel channel, IServiceScopeFactory serviceScopeFactory, IOptions<RabbitMqSettings> options)
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
                Console.WriteLine($"Error event received: {message}");

                var errorEvent = JsonSerializer.Deserialize<ErrorEvent>(message);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
                    customerService.ProcessErrorEvent(errorEvent);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _options.ErrorQueue, autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
