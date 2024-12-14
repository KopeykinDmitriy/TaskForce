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
        private readonly IMemoryCache _memoryCache; // Добавляем зависимость для кэша
        private readonly DatabaseContext _context;

        public UserService(UserRepository repository, IMemoryCache memoryCache, DatabaseContext context)
        {
            _repository = repository;
            _memoryCache = memoryCache;
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {

            string cacheKey = "all_users"; // Ключ для кеша

            // Проверяем, есть ли данные в кэше
            if (!_memoryCache.TryGetValue(cacheKey, out List<User> users))
            {
                // Если данных нет в кэше, получаем их из репозитория
                users = await _repository.GetAllUsersAsync();

                // Настроим параметры кэширования: например, кэшируем на 10 минут
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                
                // Сохраняем данные в кэш
                _memoryCache.Set(cacheKey, users, cacheOptions);
            }

            return await _repository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            // Проверяем, есть ли пользователь в кэше
            if (!_memoryCache.TryGetValue($"user_{userId}", out User user))
            {
                // Если нет, загружаем из базы данных
                user = await _context.Users
                    .FirstOrDefaultAsync(u => u.id == userId);

                if (user != null)
                {
                    // Сохраняем пользователя в кэш на 10 минут
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                    _memoryCache.Set($"user_{userId}", user, cacheOptions);
                }
            }

            return user; // Возвращаем пользователя (или null, если не найден)
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

            // Очистим кэш
            _memoryCache.Remove("all_users");

            return await _repository.AddUserAsync(user);
        }


        // Функция добавления тега пользователю
        public async Task AddTagToUserAsync(TagDto request)
        {
            // Проверяем, существует ли тег в базе данных
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == request.TagName);

            // Если тег не существует, создаем новый
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = request.TagName
                };

                _context.Tags.Add(tag);
                await _context.SaveChangesAsync(); // Сохраняем новый тег
            }

            // Проверяем, существует ли уже связь между пользователем и тегом
            var userTag = await _context.UserTags
                .FirstOrDefaultAsync(ut => ut.UserId == request.UserId && ut.TagId == tag.Id);

            // Если связи нет, создаем новую
            if (userTag == null)
            {
                userTag = new UserTags
                {
                    // Связываем с пользователем и тегом
                    UserId = request.UserId,
                    TagId = tag.Id,
                    User = await _context.Users.FindAsync(request.UserId), // Загружаем пользователя
                    Tag = tag // Устанавливаем объект тега
                };

                _context.UserTags.Add(userTag);
                await _context.SaveChangesAsync(); // Сохраняем связь
            }
        }
    }
}