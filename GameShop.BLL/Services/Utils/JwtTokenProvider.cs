using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Services.Interfaces.Utils;
using Microsoft.IdentityModel.Tokens;

namespace GameShop.BLL.Services.Utils
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private static readonly string SecretKey = "B7E7556636054C5086D4B9B7470A5D37DEECBD36F986D50FFD7A4B4230D6ED31";

        public string GenerateToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim("nickName", username),
                new Claim("userRole", role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
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
    }
}
