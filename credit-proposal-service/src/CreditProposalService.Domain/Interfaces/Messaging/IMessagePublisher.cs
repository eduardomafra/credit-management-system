namespace CreditProposalService.Domain.Interfaces.Messaging
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message);
    }
}
