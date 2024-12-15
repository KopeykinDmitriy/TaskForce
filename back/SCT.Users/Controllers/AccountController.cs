using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SCT.Users.Services;
using SCT.Users.DTOs;
using SCT.Users.Repositories;

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
        public async Task<IActionResult> RegisterUser(string login, string email, string password)
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
                        //Password = BCrypt.Net.BCrypt.HashPassword(password), // Хэширование, для получения использовать: BCrypt.Net.BCrypt.Verify(password, hashedPassword)
                        Role = "admin"
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


        //[AllowAnonymous]
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Извлекаем сохраненный id_token
            var idToken = HttpContext.GetTokenAsync("id_token").Result;

            // Проверяем, есть ли id_token
            if (string.IsNullOrEmpty(idToken))
            {
                throw new Exception("id_token is missing.");
            }

            // Указываем параметры для выхода
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/",
                Parameters = { { "id_token_hint", idToken } }
            };

            return SignOut(
                properties,
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }


        [HttpGet("HelloWorld")]
        public string GetString()
        {
            return "Hello world!";
        }
    }
}