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
        private readonly RabbitMqSettings _options;

        public MessagePublisher(ILogger<MessagePublisher> logger, IModel channel, IOptions<RabbitMqSettings> options)
        {
            _logger = logger;
            _channel = channel;
            _options = options.Value;
        }

        public void Publish<T>(T message)
        {
            try
            {
                _logger.LogInformation($"Publishing message of type {typeof(T).Name} to queue {_options.CustomerQueue}");

                var json = JsonSerializer.Serialize(message);
                _logger.LogDebug($"Serialized message: {json}");

                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: _options.CustomerQueue,
                    basicProperties: null,
                    body: body);

                _logger.LogInformation($"Message published to queue {_options.CustomerQueue}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while publishing message to queue {_options.CustomerQueue}");
                throw;
            }
            
        }
    }
}
