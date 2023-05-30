using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.AuthDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IUsersTokenService
    {
        Task AddUserTokenAsync(string nickName, string refreshToken);

        Task DeleteUserTokenAsync(string nickName);

        Task UpdateUserTokenAsync(string nickName, string refreshToken);

        Task<UserTokenReadDTO> GetRefreshTokenAsync(string userNickName);
    }
}
