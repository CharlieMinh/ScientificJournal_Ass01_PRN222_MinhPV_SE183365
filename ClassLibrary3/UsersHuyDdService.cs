using System.Security.Cryptography;
using System.Text;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV;
using ScientificJournal.Services.MinhPV.Interfaces;

namespace ScientificJournal.Services.MinhPV
{
    public class UsersHuyDdService : IUsersHuyDdService
    {
        private readonly UsersHuyDdRepository _repository = null!;

        public UsersHuyDdService()
        {
        }

        public UsersHuyDdService(UsersHuyDdRepository repository)
        {
            _repository = repository;
        }

        public async Task<UsersHuyDd?> LoginAsync(string email, string password)
        {
            var user = await _repository.GetActiveUserWithRolesByEmailAsync(email.Trim());
            if (user == null)
            {
                return null;
            }

            var passwordHash = HashPassword(password);
            if (string.Equals(user.PasswordHash, passwordHash, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(user.PasswordHash, password, StringComparison.Ordinal))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _repository.EmailExistsAsync(email.Trim());
        }

        public async Task<int> RegisterAsync(string fullName, string email, string password)
        {
            var user = new UsersHuyDd
            {
                FullName = fullName.Trim(),
                Email = email.Trim(),
                PasswordHash = HashPassword(password),
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            return await _repository.RegisterAsync(user, "LecturerStudent");
        }

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password.Trim()));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
