using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Entities
{
    public class UserTokens : BaseEntity
    {
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
