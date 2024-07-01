namespace CreditCardService.Domain.Models
{
    public class ErrorEvent
    {
        public string Microservice { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
        public string OriginalMessage { get; set; }
    }
}
