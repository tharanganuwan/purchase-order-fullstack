using PurchaseOrder.Infrastructure.Data;
using static PurchaseOrder.Domain.Interfaces.IRepository;

namespace PurchaseOrder.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context) => _context = context;

        public async Task<T?> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Remove(T entity) => _context.Set<T>().Remove(entity);
        public IQueryable<T> Query() => _context.Set<T>();
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}
