using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineTicketBookingSystem.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineTicketBookingSystem.Utility
{
    public class JwtService

    {

        private readonly JwtSettings _jwtSettings;

        public JwtService(IConfiguration configuration)

        {

            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>(); // Sử dụng Get<T>()

        }

        public string GenerateToken(User user)

        {

            var claims = new List<Claim>

        {

            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email),

            // Add additional claims as needed (e.g., roles)

        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor

            {

                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryInMinutes),

                SigningCredentials = credentials,

                Issuer = _jwtSettings.Issuer,

                Audience = _jwtSettings.Audience

            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);

        }
        public IDictionary<string, string> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var claimsDictionary = new Dictionary<string, string>();

            if (handler.CanReadToken(token) && !string.IsNullOrEmpty(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                // Lấy các thông tin (claims) từ token
                foreach (var claim in jwtToken.Claims)
                {
                    claimsDictionary[claim.Type] = claim.Value;
                }
            }
            else
            {
                throw new SecurityTokenException("Token không hợp lệ.");
            }

            return claimsDictionary;
        }
    }
}
