using CustomerService.Domain.Interfaces.Messaging;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using CustomerService.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace CustomerService.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ILogger<MessagePublisher> _logger;
        private readonly IModel _channel;

        public MessagePublisher(ILogger<MessagePublisher> logger, IModel channel)
        {
            _logger = logger;
            _channel = channel;
        }

        public void Publish<T>(T message, string routingKey = "")
        {
            try
            {
                _logger.LogInformation($"Publishing message of type {typeof(T).Name} to queue {routingKey}");

                var json = JsonSerializer.Serialize(message);
                _logger.LogDebug($"Serialized message: {json}");

                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: routingKey,
                    basicProperties: null,
                    body: body);

                _logger.LogInformation($"Message published to queue {routingKey}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while publishing message to queue {routingKey}");
                throw;
            }
            
        }
    }
}
