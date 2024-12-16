using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCT.Users.Services;
using SCT.Users.DTOs;

namespace SCT.Users.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly KeycloakService _keycloakService;
        private readonly UserService _userService;

        public AccountController(KeycloakService keycloakService, UserService userService)
        {
            _keycloakService = keycloakService ?? throw new ArgumentNullException(nameof(keycloakService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(string login, string email, string password, string role, List<string> tags)
        {
            // Получение токена администратора Keycloak
            var tokenResponse = await _keycloakService.GetAdminAccessToken();
            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return StatusCode(500, "Не удалось получить токен администратора Keycloak.");
            }

            // Создание пользователя в Keycloak
            var createUserResult = await _keycloakService.CreateUserInKeycloak(tokenResponse.AccessToken, login, email, password);

            if (createUserResult.IsSuccess)
            {
                try
                {
                    // Создание пользователя в базе данных
                    var userDto = new UserDto
                    {
                        Name = login,
                        Email = email,
                        Role = role,
                        Tags = tags
                    };

                    await _userService.AddUserAsync(userDto);

                    return Ok("Пользователь успешно зарегистрирован.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Ошибка при добавлении пользователя в базу данных: {ex.Message}");
                }
            }

            // Возврат ошибки Keycloak
            return StatusCode((int)createUserResult.StatusCode, createUserResult.ErrorResponse);
        }
    }
}