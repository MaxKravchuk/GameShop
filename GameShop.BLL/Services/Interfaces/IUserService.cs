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
        Task<bool> IsValidUserCredentialsAsync(UserCreateDTO userCreateDTO);

        Task<bool> IsAnExistingUserAsync(string nickName);

        Task<string> GetRoleAsync(string userName);

        Task<int> GetIdAsync(string nickName);

        Task CreateUserAsync(UserCreateDTO userCreateDTO);

        Task CreateUserWithRoleAsync(UserCreateWithRoleDTO userWithRoleCreateDTO);

        Task UpdateUserAsync(UserUpdateDTO userUpdateDTO);

        Task DeleteUserAsync(int userId);

        Task<IEnumerable<UserReadListDTO>> GetUsersAsync();
    }
}
