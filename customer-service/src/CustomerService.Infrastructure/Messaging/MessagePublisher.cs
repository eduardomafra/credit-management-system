using CustomerService.Domain.Interfaces.Messaging;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace CustomerService.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IModel _channel;

        public MessagePublisher(IModel channel)
        {
            _channel = channel;
        }

        public void Publish<T>(T message, string routingKey = "")
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: "",
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }
    }
}
