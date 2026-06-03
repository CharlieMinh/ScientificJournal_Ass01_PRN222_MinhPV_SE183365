using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV.Base;

namespace ScientificJournal.Repositories.MinhPV
{
    public class JournalsMinhPvRepository : GenericRepository<JournalsMinhPv>
    {
        public JournalsMinhPvRepository()
        {
        }

        public JournalsMinhPvRepository(ScientificJournalTrendDBContext context) : base(context)
        {
        }

        public async Task<List<JournalsMinhPv>> GetAllWithPublisherAsync()
        {
            return await _context.JournalsMinhPvs
                .Include(j => j.Publisher)
                .OrderBy(j => j.JournalName)
                .ToListAsync();
        }

        public async Task<JournalsMinhPv?> GetDetailAsync(int id)
        {
            return await _context.JournalsMinhPvs
                .Include(j => j.Publisher)
                .Include(j => j.CategoryIdMinhPvs)
                .Include(j => j.PapersBaoTgs)
                .FirstOrDefaultAsync(j => j.JournalIdMinhPv == id);
        }

        public async Task<JournalsMinhPv?> GetWithCategoriesAsync(int id)
        {
            return await _context.JournalsMinhPvs
                .Include(j => j.CategoryIdMinhPvs)
                .FirstOrDefaultAsync(j => j.JournalIdMinhPv == id);
        }

        public async Task<int> AssignCategoriesAsync(int journalId, List<int> categoryIds)
        {
            var journal = await GetWithCategoriesAsync(journalId);
            if (journal == null)
            {
                return 0;
            }

            var categories = await _context.CategoriesMinhPvs
                .Where(c => categoryIds.Contains(c.CategoryIdMinhPv))
                .ToListAsync();

            journal.CategoryIdMinhPvs.Clear();
            foreach (var category in categories)
            {
                journal.CategoryIdMinhPvs.Add(category);
            }

            return await _context.SaveChangesAsync();
        }
    }
}
