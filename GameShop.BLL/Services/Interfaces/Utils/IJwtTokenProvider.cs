using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces.Utils
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(string username, string role);

        ClaimsPrincipal ValidateToken(string token);
    }
}
