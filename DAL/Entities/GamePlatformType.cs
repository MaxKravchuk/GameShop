using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class GamePlatformType
    {
        public string GameKey { get; set; }
        public Game Game { get; set; }
        public string Type { get; set; }
        public PlatformType PlatformType { get; set; }
    }
}
