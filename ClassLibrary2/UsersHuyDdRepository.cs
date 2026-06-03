using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV.Base;

namespace ScientificJournal.Repositories.MinhPV
{
    public class UsersHuyDdRepository : GenericRepository<UsersHuyDd>
    {
        public UsersHuyDdRepository()
        {
        }

        public UsersHuyDdRepository(ScientificJournalTrendDBContext context) : base(context)
        {
        }

        public async Task<UsersHuyDd?> GetActiveUserWithRolesByEmailAsync(string email)
        {
            return await _context.UsersHuyDds
                .Include(u => u.RoleIdHuyDds)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.UsersHuyDds.AnyAsync(u => u.Email == email);
        }

        public async Task<RolesHuyDd?> GetRoleByNameAsync(string roleName)
        {
            return await _context.RolesHuyDds.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<int> RegisterAsync(UsersHuyDd user, string defaultRoleName)
        {
            var role = await GetRoleByNameAsync(defaultRoleName);
            if (role != null)
            {
                user.RoleIdHuyDds.Add(role);
            }

            _context.UsersHuyDds.Add(user);
            return await _context.SaveChangesAsync();
        }
    }
}
