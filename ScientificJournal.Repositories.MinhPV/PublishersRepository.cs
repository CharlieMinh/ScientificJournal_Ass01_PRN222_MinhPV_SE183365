using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV.Base;

namespace ScientificJournal.Repositories.MinhPV
{
    public class PublishersRepository : GenericRepository<Publisher>
    {
        public PublishersRepository()
        {
        }

        public PublishersRepository(ScientificJournalTrendDBContext context) : base(context)
        {
        }

        public async Task<List<Publisher>> GetAllWithJournalsAsync()
        {
            return await _context.Publishers
                .Include(p => p.JournalsMinhPvs)
                .OrderBy(p => p.PublisherName)
                .ToListAsync();
        }

        public async Task<Publisher?> GetDetailAsync(int id)
        {
            return await _context.Publishers
                .Include(p => p.JournalsMinhPvs)
                .FirstOrDefaultAsync(p => p.PublisherId == id);
        }
    }
}
