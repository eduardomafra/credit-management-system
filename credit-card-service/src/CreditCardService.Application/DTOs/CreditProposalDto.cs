namespace CreditCardService.Application.DTOs
{
    public class CreditProposalDto
    {
        public int CreditProposalId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
