namespace CustomerService.Application.DTOs
{
    public class FinancialProfileDto
    {
        public decimal MonthlyIncome { get; set; }
        public int CreditScore { get; set; }

        public FinancialProfileDto() { }
        public FinancialProfileDto(decimal monthlyIncome, int creditScore)
        {
            MonthlyIncome = monthlyIncome;
            CreditScore = creditScore;
        }
    }
}
