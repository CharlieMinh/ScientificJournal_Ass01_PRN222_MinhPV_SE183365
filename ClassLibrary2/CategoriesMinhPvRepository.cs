using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV.Base;

namespace ScientificJournal.Repositories.MinhPV
{
    public class CategoriesMinhPvRepository : GenericRepository<CategoriesMinhPv>
    {
        public CategoriesMinhPvRepository()
        {
        }

        public CategoriesMinhPvRepository(ScientificJournalTrendDBContext context) : base(context)
        {
        }

        public async Task<List<CategoriesMinhPv>> GetAllOrderedAsync()
        {
            return await _context.CategoriesMinhPvs
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<CategoriesMinhPv?> GetDetailAsync(int id)
        {
            return await _context.CategoriesMinhPvs
                .Include(c => c.JournalIdMinhPvs)
                    .ThenInclude(j => j.Publisher)
                .FirstOrDefaultAsync(c => c.CategoryIdMinhPv == id);
        }
    }
}
