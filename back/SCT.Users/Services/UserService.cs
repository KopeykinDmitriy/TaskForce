using SCT.Common.Data.Entities;
using SCT.Users.DTOs;
using SCT.Users.Providers;
using SCT.Users.Repositories;

namespace SCT.Users.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly IUsernameProvider _usernameProvider;

        public UserService(UserRepository repository, IUsernameProvider usernameProvider)
        {
            _repository = repository;
            _usernameProvider = usernameProvider;
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

        public async Task<List<UserDto>> GetAllUsersByProjectAsync(int projectId)
        {
            var users = await _repository.GetAllUsersByProjectAsync(projectId);
            
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
            await AddUserToProjectAsync(newUser.id, userDto.ProjectId);
        }

        public Task AddTagsToUserAsync(int userId, List<string> tagNames)
        {
            return _repository.AddTagsToUserAsync(userId, tagNames);
        }

        public async Task AddUserToProjectAsync(int userId, int projectId)
        {
            await _repository.AddUserToProjectAsync(userId, projectId);
        }

        public async Task<UserDto> GetUserInfo()
        {
            var name = _usernameProvider.Get();
            var user = await _repository.GetUserByLogin(name);
            return new UserDto
            {
                Name = user.name,
                Id = user.id,
                Role = user.role,
                Tags = user.UserTags.Select(ut => ut.Tag.Name).ToList()
            };
        }
    }
}