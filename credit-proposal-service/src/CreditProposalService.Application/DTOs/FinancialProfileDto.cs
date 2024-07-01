namespace CreditProposalService.Application.DTOs
{
    public class FinancialProfileDto
    {
        public int CustomerId { get; set; }
        public int FinancialProfileId { get; set; }
        public decimal MonthlyIncome { get; set; }
        public int CreditScore { get; set; }
        public bool OwnsHome { get; set; }
        public bool OwnsVehicle { get; set; }
    }
}
