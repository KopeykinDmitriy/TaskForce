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
            // URL для API Keycloak
            var keycloakBaseUrl = _configuration["Keycloak:AuthServerUrl"];
            var realm = _configuration["Keycloak:Realm"];
            var adminClientId = _configuration["Keycloak:AdminClientId"];
            var adminClientSecret = _configuration["Keycloak:AdminClientSecret"];

            // Получение admin access token
            var tokenResponse = await GetAdminAccessToken(keycloakBaseUrl, adminClientId, adminClientSecret);
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

            var response = await httpClient.PostAsync($"{keycloakUrl}/protocol/openid-connect/token", tokenRequest);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TokenResponse>(content);
        }


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
            public string? AccessToken { get; set; }
        }

        [HttpGet("HelloWorld")]
        public string GetString()
        {
            return "Hello world";
        }
    }
}
