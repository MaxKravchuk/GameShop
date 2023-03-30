using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isDeleted { get; set; } = false;

        public virtual ICollection<Game> GameGenres { get; set; } = new List<Game>();
        public string ParentGenreName { get; set; }
        public virtual Genre ParentGenre { get; set; }
        public virtual ICollection<Genre> SubGenres { get; set; } = new List<Genre>();
    }
}
