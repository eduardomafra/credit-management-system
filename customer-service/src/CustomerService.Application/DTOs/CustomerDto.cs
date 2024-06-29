namespace CustomerService.Application.DTOs
{
    public class CustomerDto
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public FinancialProfileDto FinancialProfile { get; set; }
    }
}
