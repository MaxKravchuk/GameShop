using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class GamePlatformType
    {
        public Game Game { get; set; }
        public string GameKey { get; set; }

        public PlatformType PlatformType { get; set; }
        public string PlatformTypeName { get; set; }
    }
}
