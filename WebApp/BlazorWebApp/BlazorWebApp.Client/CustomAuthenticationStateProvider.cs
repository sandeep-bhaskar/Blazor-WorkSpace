using BlazorWebApp.Client.Constants;
using BlazorWebApp.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorWebApp.Client
{
    public class CustomAuthenticationStateProvider(ICookieStorageService cookieStorageService,
          NavigationManager navigationManager,
          IUserContext userContext,
          ILogger<CustomAuthenticationStateProvider> logger) : AuthenticationStateProvider
    {

        private readonly ICookieStorageService _cookieStorageService = cookieStorageService;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly IUserContext _userContext = userContext;
        private readonly ILogger<CustomAuthenticationStateProvider> _logger = logger;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string token = await _cookieStorageService.GetItemAsync(ClientConstants.CookieStorage.AuthToken);
                if (string.IsNullOrEmpty(token))
                {
                    if (!_navigationManager.Uri.Contains("/account/login"))
                    {
                        _navigationManager.NavigateTo($"/account/login", forceLoad: true);
                    }
                    else
                    {
                        return new AuthenticationState(_anonymous);
                    }
                }
                else
                {
                    ClaimsIdentity identity = new(new JwtSecurityToken(token).Claims, "jwt");
                    ClaimsPrincipal user = new(identity);
                    if (ValidateTokenExpiry(user))
                    {
                        _userContext.SetUser(user);
                        return await Task.FromResult(new AuthenticationState(user));
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred in CustomAuthenticationStateProvider");
            }
            return new AuthenticationState(_anonymous);
        }

        public async Task<bool> Logout()
        {
            try
            {
                await _cookieStorageService.RemoveItem(ClientConstants.CookieStorage.AuthToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occurred while deleting from local storage");
            }
            return false;
        }

        private static bool ValidateTokenExpiry(ClaimsPrincipal claims)
        {
            if (claims != null)
            {
                Claim? exp = claims.FindFirst(c => c.Type.Equals("exp"));
                if (exp != null && !string.IsNullOrEmpty(exp.Value))
                {
                    string expiry = exp.Value;
                    DateTimeOffset datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry));
                    if (datetime.UtcDateTime >= DateTime.UtcNow)
                        return true;
                }
            }
            return false;
        }
    }
}
