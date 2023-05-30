using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.AuthDTOs
{
    public class AuthenticatedResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
