using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.GameDTOs
{
    public class GameCreateDTO : GameBaseDTO
    {
        public string Description { get; set; }
        public IEnumerable<int> GenresId { get; set; } = new List<int>();
        public IEnumerable<int> PlatformTypeId { get; set; } = new List<int>();
    }
}
