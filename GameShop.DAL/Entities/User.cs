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

        public int? RoleId { get; set; }

        public Publisher Publisher { get; set; }

        public int? PublisherId { get; set; }

        public UserTokens RefreshToken { get; set; }

        public int RefreshTokenId { get; set; }

        public DateTime? BannedTo { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
