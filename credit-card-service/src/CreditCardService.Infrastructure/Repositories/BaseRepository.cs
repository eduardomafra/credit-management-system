using CreditCardService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreditCardService.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly CreditCardDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(CreditCardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
