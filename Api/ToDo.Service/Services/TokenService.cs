using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ToDo.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ToDo.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        
        public string GenerateRefreshToken(int userId)
        {
            var refreshToken = new
            {
                UserId = userId,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_config.GetValue<int>("RefreshTokenExpiryDays"))
            };

            string jsonString = JsonConvert.SerializeObject(refreshToken);
            var tokenString = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
            return tokenString;
        }

        public string GenerateAccessToken(int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,userId.ToString())
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            string jsonString = Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));
            var token = JsonConvert.DeserializeObject<dynamic>(jsonString)!;
            if (token != null && token?.ExpiresAt > DateTime.UtcNow && token?.UserId != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public dynamic GenerateNewTokens(string refreshToken)
        {
            string jsonString = Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));

            var token = JsonConvert.DeserializeObject<dynamic>(jsonString)!;
        
            var response = new
            {
                jwttoken = GenerateAccessToken((int)token?.UserId),
                refreshToken = GenerateRefreshToken((int)token?.UserId)
            };
            return response;
        }
    }
}
