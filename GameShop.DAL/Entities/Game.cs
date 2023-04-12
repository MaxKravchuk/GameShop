using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Entities
{
    public class Game : BaseEntity
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Genre> GameGenres { get; set; } = new List<Genre>();
        public ICollection<PlatformType> GamePlatformTypes { get; set; } = new List<PlatformType>();
    }
}
