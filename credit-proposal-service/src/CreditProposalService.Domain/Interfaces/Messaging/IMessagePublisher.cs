using CreditProposalService.Domain.Models;

namespace CreditProposalService.Domain.Interfaces.Messaging
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message, string routingKey = "");
        void PublishErrorEvent(ErrorEvent errorEvent);
    }
}
