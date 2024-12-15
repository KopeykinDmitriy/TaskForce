namespace SCT.Users.Providers
{
    public class UsernameProvider : IUsernameProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UsernameProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Get()
        {
            var user = _contextAccessor.HttpContext?.User;
            return user?.FindFirst("preferred_username")?.Value ?? string.Empty;
        }
    }
}
