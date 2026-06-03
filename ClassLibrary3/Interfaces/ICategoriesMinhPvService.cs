using ScientificJournal.Entities.MinhPV.Models;

namespace ScientificJournal.Services.MinhPV.Interfaces
{
    public interface ICategoriesMinhPvService
    {
        Task<List<CategoriesMinhPv>> GetAllAsync();
        Task<CategoriesMinhPv?> GetByIdAsync(int id);
        Task<CategoriesMinhPv?> GetDetailAsync(int id);
        Task<int> CreateAsync(CategoriesMinhPv category);
        Task<int> UpdateAsync(CategoriesMinhPv category);
        Task<int> DeleteAsync(int id);
    }
}
