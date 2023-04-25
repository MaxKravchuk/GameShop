using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.GameDTOs;

namespace GameShop.BLL.DTO.PublisherDTOs
{
    public class PublisherReadDTO : PublisherReadListDTO
    {
        public IEnumerable<GameReadListDTO> GameReadListDTOs { get; set; } = new List<GameReadListDTO>();
    }
}
