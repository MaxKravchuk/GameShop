using GameShop.BLL.DTO.GameDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.PlatformTypeDTOs
{
    public class PlatformTypeReadDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public IEnumerable<GameReadListDTO> Games { get; set; } = new List<GameReadListDTO>();
    }
}
