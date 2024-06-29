namespace CreditCardService.Domain.Entities
{
    public class CreditCard
    {
        public int CreditCardId { get; set; }
        public int CustomerId { get; set; }
        public int CreditProposalId { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public DateTime ExpirityDate { get; set; }
        public string CVV { get; set; }
        public decimal CreditLimit { get; set; }
        public DateTime CreatedAt { get; set; }

        public CreditCard() { }

        public CreditCard(int customerId, int creditProposalId, string cardNumber, string cvv, string cardName, decimal creditLimit)
        {
            CustomerId = customerId;
            CreditProposalId = creditProposalId;
            CardNumber = cardNumber;
            CVV = cvv;
            CardName = cardName;
            ExpirityDate = DateTime.Now.AddYears(4);
            CreditLimit = creditLimit;
            CreatedAt = DateTime.Now;
        }
    }
}
