using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;
using SCT.Common.Data.Entities;
using SCT.Users.Providers;

namespace SCT.Users.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseContext _context;
        private readonly IUsernameProvider _usernameProvider; // todo Это просто пример использования, надо будет выпилить.

        public UserRepository(DatabaseContext context, IUsernameProvider usernameProvider)
        {
            _context = context;
            _usernameProvider = usernameProvider;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Метод для поиска пользователя по email (для проверки уникальности)
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.email == email);
        }
    }
}