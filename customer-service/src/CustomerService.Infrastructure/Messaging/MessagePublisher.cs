using CustomerService.Domain.Interfaces.Messaging;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using CustomerService.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace CustomerService.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IModel _channel;
        private readonly RabbitMqOptions _options;

        public MessagePublisher(IModel channel, IOptions<RabbitMqOptions> options)
        {
            _channel = channel;
            _options = options.Value;
        }

        public void Publish<T>(T message, string routingKey = "")
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _options.QueueName,
                basicProperties: null,
                body: body);
        }
    }
}
