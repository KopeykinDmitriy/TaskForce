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
            if (_users.Count == 0)
            {
                _users = await _context.Users.Include(u => u.UserTags).ThenInclude(ut => ut.Tag).Include(u => u.UserProjects).ToListAsync();
            }
            
            return _users;
        }

        public async Task<List<User>> GetAllUsersByProjectAsync(int projectId)
        {
            if (_users.Count == 0)
            {
                _users = await _context.Users.Include(u => u.UserTags).ThenInclude(ut => ut.Tag).Include(u => u.UserProjects).ToListAsync();
            }
            
            return _users.Where(u => u.UserProjects.Any(up => up.ProjectId == projectId)).ToList();
        }

        public async Task<User> GetUserByLogin(string login)
        {
            if (_users.Count == 0)
            {
                _users = await _context.Users.Include(u => u.UserTags).ThenInclude(ut => ut.Tag).Include(u => u.UserProjects).ToListAsync();
            }
            
            return _users.FirstOrDefault(u => string.Equals(u.name.ToLower(), login.ToLower()));
        }

        /// <summary>
        /// Метод для добавления нового пользователя
        /// </summary>
        public async Task<User> AddUserAsync(User user)
        {
            var newUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _users.Add(newUser.Entity);
            return user;
        }

        public async Task AddUserToProjectAsync(int userId, int projectId)
        {
            var userProject = new UserProject { UserId = userId, ProjectId = projectId };
            _context.UserProjects.Add(userProject);
            await _context.SaveChangesAsync();
            var ind = _users.FindIndex(u => u.id == userId);
            _users[ind].UserProjects.Add(userProject);
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
