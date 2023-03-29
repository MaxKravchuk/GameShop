using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class PlatformType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool isDeleted { get; set; } = false;

        public ICollection<GamePlatformType> GamePlatformTypes { get; set; }
    }
}
