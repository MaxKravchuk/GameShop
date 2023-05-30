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
        Task<bool> IsValidUserCredentialsAsync(UserBaseDTO userBaseDTO);

        Task<bool> IsAnExistingUserAsync(string nickName);

        Task<string> GetRoleAsync(string nickName);

        Task CreateUserAsync(UserCreateDTO userCreateDTO);

        Task CreateUserWithRoleAsync(UserCreateDTO userCreateDTO);

        Task UpdateUserAsync(UserUpdateDTO userUpdateDTO);

        Task DeleteUserAsync(int userId);

        Task<UserReadDTO> GetUserByIdAsync(int userId);

        Task<IEnumerable<UserReadListDTO>> GetUsersAsync();
    }
}
