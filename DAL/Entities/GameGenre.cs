using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class GameGenre
    {
        public Game Game { get; set; }
        public string GameKey { get; set; }

        public Genre Genre { get; set; }
        public string GenreName { get; set; }
    }
}
