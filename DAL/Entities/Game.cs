using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Game : BaseEntity
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Coment> Coments { get; set; } = new List<Coment>();
        public virtual ICollection<Genre> GameGenres { get; set; } = new List<Genre>();
        public virtual ICollection<PlatformType> GamePlatformTypes{ get; set; } = new List<PlatformType>();
    }
}
