using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Entities
{
    public class User : BaseEntity
    {
        public string NickName { get; set; }

        public string PasswordHash { get; set; }

        public Role UserRole { get; set; }

        public int RoleId { get; set; }
    }
}
