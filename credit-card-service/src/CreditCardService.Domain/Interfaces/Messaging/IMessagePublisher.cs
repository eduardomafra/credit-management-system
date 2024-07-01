using CreditCardService.Domain.Models;

namespace CreditCardService.Domain.Interfaces.Messaging
{
    public interface IMessagePublisher
    {
        void PublishErrorEvent(ErrorEvent errorEvent);
    }
}
