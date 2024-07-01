using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces.Repositories;
using CustomerService.Infrastructure.Data;

namespace CustomerService.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CustomerDbContext context) : base(context)
        {
        }
    }
}
