using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorWebApp.Helpers
{
    public class CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
    {
        private Task<AuthenticationState> _authenticationStateTask;
        private HttpContext _httpContext = httpContextAccessor.HttpContext;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(_httpContext.User.Identity)));
        }

        public void SetAuthenticationState(Task<AuthenticationState> authenticationStateTask)
        {
            _authenticationStateTask = authenticationStateTask ?? throw new ArgumentNullException(nameof(authenticationStateTask));
            NotifyAuthenticationStateChanged(_authenticationStateTask);
        }
    }
}
