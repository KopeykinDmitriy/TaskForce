using SCT.Common.Data.Entities;
using SCT.Users.DTOs;
using SCT.Users.Repositories;
using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;

namespace SCT.Users.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly DatabaseContext _context;

        public UserService(UserRepository repository, DatabaseContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {

            var users = await _repository.GetAllUsersAsync();

            return users;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.id == userId);

            return user;
        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                name = userDto.Name,
                surname = userDto.Surname,
                email = userDto.Email,
                //password = userDto.Password,
                role = userDto.Role
            };

            return await _repository.AddUserAsync(user);
        }

        public Task AddTagsToUserAsync(int userId, List<string> tagNames)
        {
            return _repository.AddTagsToUserAsync(userId, tagNames);
        }
    }
}