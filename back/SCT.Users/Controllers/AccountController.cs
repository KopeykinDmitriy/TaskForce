using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SCT.Users.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(string login, string email, string password)
        {
            // Получаем настройки из конфигурации
            var keycloakBaseUrl = _configuration["Authentication:Authority"];
            var realm = _configuration["Authentication:Realm"];
            var clientId = _configuration["Authentication:ClientId"];
            var clientSecret = _configuration["Authentication:ClientSecret"];

            Console.WriteLine($"Response Status: {response.StatusCode}");
            // Получаем токен администратора для взаимодействия с Keycloak Admin API
            var tokenResponse = await GetAdminAccessToken(keycloakBaseUrl, clientId, clientSecret);
            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return StatusCode(500, "Не удалось получить токен администратора Keycloak.");
            }

            // Создание нового пользователя через Keycloak Admin API
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            var createUserPayload = new
            {
                username = login,
                email = email,
                enabled = true,
                credentials = new[]
                {
                new
                    {
                        type = "password",
                        value = password,
                        temporary = false
                    }
                }   
            };

            var response = await httpClient.PostAsync(
                $"{keycloakBaseUrl}/admin/realms/{realm}/users",
                new StringContent(JsonSerializer.Serialize(createUserPayload), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return Ok("Пользователь успешно зарегистрирован.");
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, errorResponse);
        }

        private async Task<TokenResponse?> GetAdminAccessToken(string keycloakUrl, string clientId, string clientSecret)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            });

            var uri = new Uri($"{keycloakUrl}/protocol/openid-connect/token"); // Абсолютный URI

            var response = await httpClient.PostAsync(uri, tokenRequest);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TokenResponse>(content);
        }


        [AllowAnonymous]
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

        public class TokenResponse
        {
            public string AccessToken { get; set; }
        }

        [HttpGet("HelloWorld")]
        public string GetString()
        {
            return "Hello world";
        }
    }
}
