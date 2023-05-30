using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.UserDTOs;

namespace GameShop.BLL.DTO.RoleDTOs
{
    public class RoleReadDTO : RoleBaseDTO
    {
        public int Id { get; set; }

        public IEnumerable<UserReadListDTO> UsersOfRole { get; set; }
    }
}
