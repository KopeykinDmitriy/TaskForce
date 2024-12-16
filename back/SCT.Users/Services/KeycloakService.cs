using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SCT.Users.Services
{
    public class KeycloakService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public KeycloakService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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


        public async Task<TokenResponse?> GetAdminAccessToken()
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


        public async Task<(bool IsSuccess, HttpStatusCode StatusCode, string? ErrorResponse)> CreateUserInKeycloak(string accessToken, string login, string email, string password)
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
                        value = BCrypt.Net.BCrypt.HashPassword(password), 
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
    }
}
