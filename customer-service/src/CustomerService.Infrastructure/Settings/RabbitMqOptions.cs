namespace CustomerService.Infrastructure.Settings
{
    public class RabbitMqOptions
    {
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
