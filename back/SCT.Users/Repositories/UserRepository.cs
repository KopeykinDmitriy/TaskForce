using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;
using SCT.Common.Data.Entities;

namespace SCT.Users.Repositories
{
    public class UserRepository
    {
        private List<User> _users = new();
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод для получения всех пользователей
        /// </summary>
        public async Task<List<User>> GetAllUsersAsync()
        {
            if (!_users.Any())
            {
                _users = await _context.Users.Include(u => u.UserTags).ThenInclude(ut => ut.Tag).ToListAsync();
            }

            return _users;
        }

        /// <summary>
        /// Метод для добавления нового пользователя
        /// </summary>
        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _users.Add(user);
            return user;
        }

        /// <summary>
        /// Метод для поиска пользователя по email
        /// </summary>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.email == email);

            if (user == null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);

                if (user != null && !_users.Contains(user))
                {
                    _users.Add(user);
                }
            }

            return user;
        }

        /// <summary>
        /// Метод для поиска пользователя по ID
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var user = _users.FirstOrDefault(u => u.id == userId);

            if (user == null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.id == userId);

                if (user != null && !_users.Contains(user))
                {
                    _users.Add(user); 
                }
            }

            return user;
        }

        /// <summary>
        /// Метод создания тегов для пользователя
        /// </summary>
        public async Task AddTagsToUserAsync(int userId, List<string> tagNames)
        {
            foreach (var tagName in tagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);

                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = tagName
                    };

                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }

                var userTag = await _context.UserTags
                    .FirstOrDefaultAsync(ut => ut.UserId == userId && ut.TagId == tag.Id);

                if (userTag == null)
                {
                    userTag = new UserTags
                    {
                        UserId = userId,
                        TagId = tag.Id,
                        User = await _context.Users.FindAsync(userId),
                        Tag = tag
                    };

                    _context.UserTags.Add(userTag);
                    await _context.SaveChangesAsync(); 
                }
            }
        }
    }
}
