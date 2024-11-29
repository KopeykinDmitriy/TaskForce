using DatabaseService.Data.Entities;
using DatabaseService.DTOs;
using DatabaseService.Repositories;

namespace DatabaseService.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email
            };
            return await _repository.AddUserAsync(user);
        }
    }
}