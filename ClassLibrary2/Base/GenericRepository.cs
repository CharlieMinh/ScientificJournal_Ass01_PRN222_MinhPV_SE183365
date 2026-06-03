using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;

namespace ScientificJournal.Repositories.MinhPV.Base
{
    public class GenericRepository<T> where T : class
    {
        protected ScientificJournalTrendDBContext _context;

        public GenericRepository()
        {
            _context ??= new ScientificJournalTrendDBContext();
        }

        public GenericRepository(ScientificJournalTrendDBContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> CreateAsync(T entity)
        {
            _context.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _context.ChangeTracker.Clear();
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
