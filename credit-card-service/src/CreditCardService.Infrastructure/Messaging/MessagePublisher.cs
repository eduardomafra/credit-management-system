using CreditCardService.Domain.Interfaces.Messaging;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using CreditCardService.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CreditCardService.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly ILogger<MessagePublisher> _logger;
        private readonly IModel _channel;

        public MessagePublisher(ILogger<MessagePublisher> logger,
            IModel channel)
        {
            _logger = logger;
            _channel = channel;
        }

        public void PublishErrorEvent(ErrorEvent errorEvent)
        {
            var json = JsonSerializer.Serialize(errorEvent);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "error-exchange",
                                  routingKey: "error-routing-key",
                                  basicProperties: properties,
                                  body: body);

            _logger.LogError($"Error event published to error exchange: {json}");
        }
    }
}
