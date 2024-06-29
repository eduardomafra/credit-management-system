using CreditProposalService.Application.Settings;
using CreditProposalService.Domain.Interfaces.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace CreditProposalService.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IModel _channel;
        private readonly RabbitMqSettings _options;

        public MessagePublisher(IModel channel, IOptions<RabbitMqSettings> options)
        {
            _channel = channel;
            _options = options.Value;
        }

        public void Publish<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _options.CreditProposalQueue,
                basicProperties: null,
                body: body);
        }
    }
}
