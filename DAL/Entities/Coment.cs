using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Coment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string ParentName { get; set; } = null;

        public Game Game { get; set; }
        public int GameId { get; set; }

    }
}
