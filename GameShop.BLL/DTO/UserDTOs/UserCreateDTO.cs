﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.UserDTOs
{
    public class UserCreateDTO
    {
        public string NickName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
