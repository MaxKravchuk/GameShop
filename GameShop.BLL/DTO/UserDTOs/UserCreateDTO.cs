﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.UserDTOs
{
    public class UserCreateDTO : UserBaseDTO
    {
        public string Role { get; set; }
    }
}