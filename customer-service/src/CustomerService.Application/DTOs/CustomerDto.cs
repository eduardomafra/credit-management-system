namespace CustomerService.Application.DTOs
{
    public class CustomerDto
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressDto Address { get; set; }
        public FinancialProfileDto FinancialProfile { get; set; }

        public CustomerDto() { }
        public CustomerDto(string name, string document, DateTime birthDate, string email, string phone, AddressDto address, FinancialProfileDto financialProfile)
        {
            Name = name;
            Document = document;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
            Address = address;
            FinancialProfile = financialProfile;
        }
    }
}
