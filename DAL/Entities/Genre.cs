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
        public string ParetnName { get; set; } = null;

        public ICollection<GameGenre> GameGenres { get; set; }
    }
}
