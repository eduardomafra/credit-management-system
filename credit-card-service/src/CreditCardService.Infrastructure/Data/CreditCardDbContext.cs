using CreditCardService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditCardService.Infrastructure.Data
{
    public class CreditCardDbContext : DbContext
    {
        public CreditCardDbContext(DbContextOptions<CreditCardDbContext> options) : base(options) { }

        public DbSet<CreditCard> CreditCards { get; set; }
    }
}
