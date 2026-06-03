using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV;
using ScientificJournal.Services.MinhPV.Interfaces;

namespace ScientificJournal.Services.MinhPV
{
    public class JournalsMinhPvService : IJournalsMinhPvService
    {
        private readonly JournalsMinhPvRepository _repository = null!;

        public JournalsMinhPvService()
        {
        }

        public JournalsMinhPvService(JournalsMinhPvRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JournalsMinhPv>> GetAllAsync()
        {
            return await _repository.GetAllWithPublisherAsync();
        }

        public async Task<JournalsMinhPv?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<JournalsMinhPv?> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

        public async Task<JournalsMinhPv?> GetWithCategoriesAsync(int id)
        {
            return await _repository.GetWithCategoriesAsync(id);
        }

        public async Task<int> CreateAsync(JournalsMinhPv journal)
        {
            return await _repository.CreateAsync(journal);
        }

        public async Task<int> UpdateAsync(JournalsMinhPv journal)
        {
            return await _repository.UpdateAsync(journal);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var journal = await _repository.GetByIdAsync(id);
            if (journal == null)
            {
                return 0;
            }

            return await _repository.RemoveAsync(journal) ? 1 : 0;
        }

        public async Task<int> AssignCategoriesAsync(int journalId, List<int> categoryIds)
        {
            return await _repository.AssignCategoriesAsync(journalId, categoryIds);
        }
    }
}
