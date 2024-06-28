namespace CustomerService.Domain.Entities
{
    public class FinancialProfile
    {
        public int FinancialProfileId { get; set; }        
        public decimal MonthlyIncome { get; set; }
        public int CreditScore { get; set; }
        public bool OwnsHome { get; set; }
        public bool OwnsVeicule { get; set; }
        public int CustomerId { get; set; }
    }
}
