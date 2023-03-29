using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Coment> Coments { get; set; } = new List<Coment>();
        public ICollection<GameGenre> GameGenres { get; set; } = new List<GameGenre>();
        public ICollection<GamePlatformType> GamePlatformTypes{ get; set; } = new List<GamePlatformType>();
    }
}
