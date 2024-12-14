using SCT.Common.Data.Entities;
using SCT.Users.DTOs;
using SCT.Users.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using SCT.Common.Data.DatabaseContext;

namespace SCT.Users.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly IMemoryCache _memoryCache; // ��������� ����������� ��� ����
        private readonly DatabaseContext _context;

        public UserService(UserRepository repository, IMemoryCache memoryCache, DatabaseContext context)
        {
            _repository = repository;
            _memoryCache = memoryCache;
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {

            string cacheKey = "all_users"; // ���� ��� ����

            // ���������, ���� �� ������ � ����
            if (!_memoryCache.TryGetValue(cacheKey, out List<User> users))
            {
                // ���� ������ ��� � ����, �������� �� �� �����������
                users = await _repository.GetAllUsersAsync();

                // �������� ��������� �����������: ��������, �������� �� 10 �����
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                
                // ��������� ������ � ���
                _memoryCache.Set(cacheKey, users, cacheOptions);
            }

            return await _repository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            // ���������, ���� �� ������������ � ����
            if (!_memoryCache.TryGetValue($"user_{userId}", out User user))
            {
                // ���� ���, ��������� �� ���� ������
                user = await _context.Users
                    .FirstOrDefaultAsync(u => u.id == userId);

                if (user != null)
                {
                    // ��������� ������������ � ��� �� 10 �����
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                    _memoryCache.Set($"user_{userId}", user, cacheOptions);
                }
            }

            return user; // ���������� ������������ (��� null, ���� �� ������)
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

            // ������� ���
            _memoryCache.Remove("all_users");

            return await _repository.AddUserAsync(user);
        }
    }
}