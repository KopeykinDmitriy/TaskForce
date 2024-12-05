using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Authorization.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Login()
        {
            // Пользователь уже авторизован, перенаправляем на главную страницу
            return RedirectToAction("Index", "Home");
        }

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
    }
}
