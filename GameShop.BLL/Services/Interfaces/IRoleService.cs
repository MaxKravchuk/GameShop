using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.RoleDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        Task CreateRoleAsync(RoleCreateDTO roleBaseDTO);

        Task DeleteRoleAsync(int roleId);

        Task<PagedListDTO<RoleReadListDTO>> GetRolesPagedAsync(PaginationRequestDTO paginationRequestDTO);

        Task<IEnumerable<RoleReadListDTO>> GetRolesAsync();
    }
}
