namespace CreditProposalService.Domain.Entities
{
    public class CreditProposal
    {
        public int CreditProposalId { get; set; }
        public int CustomerId { get; set; }
        public int FinancialProfileId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public CreditProposal() { }
        public CreditProposal(int customerId, int financialProfileId, decimal amount)
        {
            CustomerId = customerId;
            FinancialProfileId = financialProfileId;
            Amount = amount;
            CreatedAt = DateTime.Now;
        }
    }
}
