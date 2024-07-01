namespace CustomerService.Application.DTOs
{
    public class FinancialProfileDto
    {
        public decimal MonthlyIncome { get; set; }
        public int CreditScore { get; set; }
        public bool OwnsHome { get; set; }
        public bool OwnsVehicle { get; set; }

        public FinancialProfileDto() { }
        public FinancialProfileDto(decimal monthlyIncome, int creditScore)
        {
            MonthlyIncome = monthlyIncome;
            CreditScore = creditScore;
        }
    }
}
