using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV;
using ScientificJournal.Services.MinhPV.Interfaces;

namespace ScientificJournal.Services.MinhPV
{
    public class PublishersService : IPublishersService
    {
        private readonly PublishersRepository _repository = null!;

        public PublishersService()
        {
        }

        public PublishersService(PublishersRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Publisher>> GetAllAsync()
        {
            return await _repository.GetAllWithJournalsAsync();
        }

        public async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Publisher?> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

        public async Task<int> CreateAsync(Publisher publisher)
        {
            return await _repository.CreateAsync(publisher);
        }

        public async Task<int> UpdateAsync(Publisher publisher)
        {
            return await _repository.UpdateAsync(publisher);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var publisher = await _repository.GetByIdAsync(id);
            if (publisher == null)
            {
                return 0;
            }

            return await _repository.RemoveAsync(publisher) ? 1 : 0;
        }
    }
}
