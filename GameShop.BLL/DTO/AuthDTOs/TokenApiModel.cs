using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.AuthDTOs
{
    public class TokenApiModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
