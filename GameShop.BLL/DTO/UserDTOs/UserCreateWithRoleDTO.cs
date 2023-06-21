using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.UserDTOs
{
    public class UserCreateWithRoleDTO : UserCreateDTO
    {
        public int RoleId { get; set; }

        public int? PublisherId { get; set; }
    }
}
