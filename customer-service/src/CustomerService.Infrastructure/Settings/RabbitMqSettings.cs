﻿namespace CustomerService.Infrastructure.Settings
{
    public class RabbitMqSettings
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CustomerQueue { get; set; }
        public string ErrorQueue { get; set; }
    }
}