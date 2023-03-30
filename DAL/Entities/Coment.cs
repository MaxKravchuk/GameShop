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
        public bool isDeleted { get; set; } = false;

        public virtual Game Game { get; set; }
        public string GameKey { get; set; }

    }
}
