using CustomerService.Application.DTOs;
using FluentValidation;

namespace CustomerService.Application.Validators
{
    public class CustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CustomerDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(x => x.Document).NotEmpty().WithMessage("Documento é obrigatório.");
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Data de nascimento é obrigatório.")
                                    .LessThan(DateTime.Now).WithMessage("Data de nascimento inválida.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email é obrigatório.")
                                 .EmailAddress().WithMessage("Email invalido.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Contato telefônico é obrigatório.");
            RuleFor(x => x.FinancialProfile).NotNull().WithMessage("Financial Profile is required.");
        }
    }
}
