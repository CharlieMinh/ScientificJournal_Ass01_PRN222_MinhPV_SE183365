using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV;
using ScientificJournal.Services.MinhPV.Interfaces;

namespace ScientificJournal.Services.MinhPV
{
    public class CategoriesMinhPvService : ICategoriesMinhPvService
    {
        private readonly CategoriesMinhPvRepository _repository = null!;

        public CategoriesMinhPvService()
        {
        }

        public CategoriesMinhPvService(CategoriesMinhPvRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoriesMinhPv>> GetAllAsync()
        {
            return await _repository.GetAllOrderedAsync();
        }

        public async Task<CategoriesMinhPv?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<CategoriesMinhPv?> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

        public async Task<int> CreateAsync(CategoriesMinhPv category)
        {
            return await _repository.CreateAsync(category);
        }

        public async Task<int> UpdateAsync(CategoriesMinhPv category)
        {
            return await _repository.UpdateAsync(category);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                return 0;
            }

            return await _repository.RemoveAsync(category) ? 1 : 0;
        }
    }
}
