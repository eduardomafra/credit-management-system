using CreditCardService.Application.DTOs;
using CreditCardService.Application.Interfaces.Services;
using CreditCardService.Domain.Interfaces.Messaging;
using CreditCardService.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CreditCardService.Infrastructure.Messaging
{
    public class MessageConsumer : BackgroundService
    {
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessageConsumer(ILogger<MessageConsumer> logger,
            IModel channel, 
            IServiceScopeFactory serviceScopeFactory)
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
                _logger.LogInformation($"Message received: {message}");

                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(new[]
                    {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                    }, onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Waiting {timeSpan} before next retry.");
                    });

                try
                {
                    await policy.ExecuteAsync(async () =>
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var creditProposalService = scope.ServiceProvider.GetRequiredService<ICreditCardService>();
                            var creditProposal = JsonSerializer.Deserialize<CreditProposalDto>(message);
                            await creditProposalService.ProcessCreditCard(creditProposal);
                        }
                    });

                    _channel.BasicAck(ea.DeliveryTag, false);
                    _logger.LogInformation($"Message processed and acknowledged: {message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing message: {message}");

                    var errorEvent = new ErrorEvent
                    {
                        Microservice = "CreditCardService",
                        ErrorMessage = ex.Message,
                        Timestamp = DateTime.UtcNow,
                        OriginalMessage = message
                    };

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();
                        messagePublisher.PublishErrorEvent(errorEvent);
                    }

                    var properties = _channel.CreateBasicProperties();
                    properties.Persistent = true;

                    _channel.BasicPublish(
                        exchange: "dead-letter-exchange",
                        routingKey: "dead-letter-routing-key",
                        basicProperties: properties,
                        body: ea.Body);

                    _channel.BasicAck(ea.DeliveryTag, false);
                    _logger.LogInformation($"Message published to dead-letter exchange: {message}");
                }
            };

            _channel.BasicConsume(queue: "credit-proposal-queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
