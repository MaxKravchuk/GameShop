using System.Collections.Generic;
using GameShop.BLL.DTO.GameDTOs;

namespace GameShop.BLL.DTO.PlatformTypeDTOs
{
    public class PlatformTypeReadDTO
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public IEnumerable<GameReadListDTO> Games { get; set; } = new List<GameReadListDTO>();
    }
}
