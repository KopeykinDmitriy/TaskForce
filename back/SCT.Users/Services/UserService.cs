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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                name = userDto.Name,
                surname = userDto.Surname,
                email = userDto.Email,
                password = userDto.Password,
                //role = userDto.Role
            };

            return await _repository.AddUserAsync(user);
        }
    }
}