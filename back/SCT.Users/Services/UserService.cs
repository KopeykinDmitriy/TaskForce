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
                password = userDto.Password,
                //role = userDto.Role
            };

            return await _repository.AddUserAsync(user);
        }


        public async Task AddTagsToUserAsync(List<TagDto> tagDtos)
        {
            foreach (var tagDto in tagDtos)
            {
                var user = await _context.Users.FindAsync(tagDto.UserId);
                if (user == null)
                {
                    throw new Exception($"User with ID {tagDto.UserId} not found");
                }

                var tag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.Name == tagDto.TagName);

                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = tagDto.TagName
                    };

                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync(); 
                }

                var userTag = await _context.UserTags
                    .FirstOrDefaultAsync(ut => ut.UserId == tagDto.UserId && ut.TagId == tag.Id);

                if (userTag == null)
                {
                    userTag = new UserTags
                    {
                        UserId = tagDto.UserId,
                        TagId = tag.Id,
                        User = user,
                        Tag = tag
                    };

                    _context.UserTags.Add(userTag);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}