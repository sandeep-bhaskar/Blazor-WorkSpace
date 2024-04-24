using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorWebApp.Components.Pages
{
    public partial class Login
    {
        [SupplyParameterFromForm]
        public LoginModel? Model { get; set; }

        [CascadingParameter]
        public HttpContext HttpContext { get; set; }

        protected override void OnInitialized() => Model ??= new();

        private async Task Submit()
        {
            if (Model.UserName == "BlazorDeveloper" && Model.Password == "NowYouSeeMe")
            {
                string token = GenerateToken();
                HttpContext.Response.Cookies.Append("X-Access-Token", token);
                navigationManager.NavigateTo("");
            }
        }

        private string GenerateToken()
        {
            List<Claim> claims = new()
                {
                    new Claim("email", "blazorDeveloper@blazor.com"),
                    new Claim(ClaimTypes.Name, "BlazorDeveloper"),
                    new Claim("id", "1"),
                    new Claim(ClaimTypes.Role, "BlazorDeveloper")
                };

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
