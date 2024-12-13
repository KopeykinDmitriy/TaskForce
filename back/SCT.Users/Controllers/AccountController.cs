using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
            var tokenResponse = await GetAdminAccessToken();
            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return StatusCode(500, "Не удалось получить токен администратора Keycloak.");
            }

            var createUserResult = await CreateUserInKeycloak(tokenResponse.AccessToken, login, email, password);

            if (createUserResult.IsSuccess)
            {
                return Ok("Пользователь успешно зарегистрирован.");
            }

            return StatusCode((int)createUserResult.StatusCode, createUserResult.ErrorResponse);
        }

        private async Task<TokenResponse?> GetAdminAccessToken()
        {
            var keycloakUrl = _configuration["Authentication:Authority"];
            var clientId = _configuration["Authentication:ClientId"];
            var clientSecret = _configuration["Authentication:ClientSecret"];
            var username = _configuration["Authentication:Name"];
            var password = _configuration["Authentication:Password"];

            var httpClient = _httpClientFactory.CreateClient();

            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await httpClient.PostAsync($"{keycloakUrl.TrimEnd('/')}/protocol/openid-connect/token", tokenRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Ошибка при получении токена: {error}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TokenResponse>(content);
        }

        private async Task<(bool IsSuccess, HttpStatusCode StatusCode, string? ErrorResponse)> CreateUserInKeycloak(string accessToken, string login, string email, string password)
        {
            var keycloakAdminUrl = _configuration["Authentication:AuthorityAdmin"];
            var realm = _configuration["Authentication:Realm"];

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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
                $"{keycloakAdminUrl.TrimEnd('/')}/{realm}/users", 
                new StringContent(JsonSerializer.Serialize(createUserPayload), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return (true, response.StatusCode, null);
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            return (false, response.StatusCode, errorResponse);
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
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; }
        }


        [HttpGet("HelloWorld")]
        public string GetString()
        {
            return "Hello world";
        }
    }
}


