using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.UserDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsValidUserCredentials(UserBaseDTO userBaseDTO);

        Task<bool> IsAnExistingUser(string nickName);

        Task<string> GetRoleAsync(string nickName);

        Task CreateUser(UserCreateDTO userCreateDTO);

        void UpdateUser(UserUpdateDTO userUpdateDTO);

        void DeleteUser(string login);
    }
}
