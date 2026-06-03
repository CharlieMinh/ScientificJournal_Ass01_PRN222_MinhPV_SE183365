using ScientificJournal.Entities.MinhPV.Models;

namespace ScientificJournal.Services.MinhPV.Interfaces
{
    public interface IPublishersService
    {
        Task<List<Publisher>> GetAllAsync();
        Task<Publisher?> GetByIdAsync(int id);
        Task<Publisher?> GetDetailAsync(int id);
        Task<int> CreateAsync(Publisher publisher);
        Task<int> UpdateAsync(Publisher publisher);
        Task<int> DeleteAsync(int id);
    }
}
