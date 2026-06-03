using ScientificJournal.Entities.MinhPV.Models;

namespace ScientificJournal.Services.MinhPV.Interfaces
{
    public interface IUsersHuyDdService
    {
        Task<UsersHuyDd?> LoginAsync(string email, string password);
        Task<bool> EmailExistsAsync(string email);
        Task<int> RegisterAsync(string fullName, string email, string password);
    }
}
