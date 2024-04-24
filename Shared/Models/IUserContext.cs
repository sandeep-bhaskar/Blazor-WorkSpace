using System.Security.Claims;

namespace Models
{
    public interface IUserContext
    {
        public string? Email { get; set; }
        public string? Id { get; set; }

        void SetUser(ClaimsPrincipal claims);
    }
}
