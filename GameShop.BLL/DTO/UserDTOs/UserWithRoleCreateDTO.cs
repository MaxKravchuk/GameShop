using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.UserDTOs
{
    public class UserWithRoleCreateDTO : UserCreateDTO
    {
        public int RoleId { get; set; }
    }
}
