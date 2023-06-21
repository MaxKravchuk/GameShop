using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.AuthDTOs;

namespace GameShop.BLL.Services.Interfaces.Utils
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(int id, string username, string role);

        string GenerateRefreshToken();

        ClaimsPrincipal ValidateToken(string token);

        AuthenticatedResponse GetAuthenticatedResponse(int id, string username, string role);
    }
}
