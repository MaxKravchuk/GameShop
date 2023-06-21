using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.AuthDTOs;
using GameShop.BLL.DTO.UserDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IUsersTokenService
    {
        Task<AuthenticatedResponse> AddUserTokenAsync(UserCreateDTO userCreateDTO);

        Task DeleteUserTokenAsync(string nickName);

        Task<AuthenticatedResponse> UpdateUserTokenAsync(string oldRefreshToken);
    }
}
