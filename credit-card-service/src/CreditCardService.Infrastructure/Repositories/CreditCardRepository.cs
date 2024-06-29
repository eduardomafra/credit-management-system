using CreditCardService.Domain.Entities;
using CreditCardService.Domain.Interfaces.Repositories;
using CreditCardService.Infrastructure.Data;

namespace CreditCardService.Infrastructure.Repositories
{
    public class CreditCardRepository : BaseRepository<CreditCard>, ICreditCardRepository
    {
        public CreditCardRepository(CreditCardDbContext context) : base(context)
        {
        }
    }
}
