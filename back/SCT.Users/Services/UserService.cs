using SCT.Common.Data.Entities;
using SCT.Users.DTOs;
using SCT.Users.Repositories;

namespace SCT.Users.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllUsersAsync();

            return users.Select(u => new UserDto
            {
                Name = u.name,
                Id = u.id,
                Role = u.role,
                Tags = u.UserTags.Select(ut => ut.Tag.Name).ToList()
            }).ToList();
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                name = userDto.Name,
                surname = userDto.Surname,
                email = userDto.Email,
                role = userDto.Role
            };
            var newUser = await _repository.AddUserAsync(user);
            await AddTagsToUserAsync(newUser.id, userDto.Tags);
        }

        public Task AddTagsToUserAsync(int userId, List<string> tagNames)
        {
            return _repository.AddTagsToUserAsync(userId, tagNames);
        }
    }
}