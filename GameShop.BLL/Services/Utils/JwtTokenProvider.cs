using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.AuthDTOs;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Services.Interfaces.Utils;
using Microsoft.IdentityModel.Tokens;

namespace GameShop.BLL.Services.Utils
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly string _secretKey = ConfigurationManager.AppSettings["SecretKey"];
        private readonly int _expMinutes = int.Parse(ConfigurationManager.AppSettings["ExpirationMinutes"]);

        public string GenerateToken(int id, string username, string role)
        {
            var claims = new[]
            {
                new Claim("Id", id.ToString()),
                new Claim("UserName", username),
                new Claim("Role", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expMinutes),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.FromMinutes(_expMinutes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AuthenticatedResponse GetAuthenticatedResponse(int id, string username, string role)
        {
            var accessToken = GenerateToken(id, username, role);
            var refreshToken = GenerateRefreshToken();

            var responseModel = new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken,
            };

            return responseModel;
        }
    }
}
