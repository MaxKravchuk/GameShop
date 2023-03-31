using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public string ParentGenreName { get; set; }
        public virtual Genre ParentGenre { get; set; }
        public virtual ICollection<Genre> SubGenres { get; set; } = new List<Genre>();
        public virtual ICollection<Game> GameGenres { get; set; } = new List<Game>();
    }
}
