using GameShop.BLL.DTO.ComentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;

namespace GameShop.BLL.DTO.GameDTOs
{
    public class GameReadDTO : GameBaseDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<GenreReadListDTO> Genres { get; set; }
        public IEnumerable<PlatformTypeReadListDTO> PlatformTypes { get; set; }

    }
}
