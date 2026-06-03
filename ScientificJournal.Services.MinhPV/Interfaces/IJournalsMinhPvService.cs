using ScientificJournal.Entities.MinhPV.Models;

namespace ScientificJournal.Services.MinhPV.Interfaces
{
    public interface IJournalsMinhPvService
    {
        Task<List<JournalsMinhPv>> GetAllAsync();
        Task<JournalsMinhPv?> GetByIdAsync(int id);
        Task<JournalsMinhPv?> GetDetailAsync(int id);
        Task<JournalsMinhPv?> GetWithCategoriesAsync(int id);
        Task<int> CreateAsync(JournalsMinhPv journal);
        Task<int> UpdateAsync(JournalsMinhPv journal);
        Task<int> DeleteAsync(int id);
        Task<int> AssignCategoriesAsync(int journalId, List<int> categoryIds);
    }
}
