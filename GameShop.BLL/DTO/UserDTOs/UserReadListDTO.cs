using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.UserDTOs
{
    public class UserReadListDTO : UserBaseDTO
    {
        public string NickName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
