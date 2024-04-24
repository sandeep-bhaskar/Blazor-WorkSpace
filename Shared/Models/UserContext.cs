using System.Security.Claims;

namespace Models
{
    public class UserContext : IUserContext
    {
        public string? Email { get; set; }
        public string? Id { get; set; }

        public void SetUser(ClaimsPrincipal claims)
        {
            if (claims.Identity!.IsAuthenticated)
            {
                this.Email = claims.FindFirst("email")?.Value;
                this.Id = claims.FindFirst("id")?.Value;
            }
        }
    }
}
