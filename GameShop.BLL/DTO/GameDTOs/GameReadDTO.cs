using System.Collections.Generic;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.DTO.PublisherDTOs;

namespace GameShop.BLL.DTO.GameDTOs
{
    public class GameReadDTO : GameBaseDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public PublisherReadListDTO PublisherReadDTO { get; set; }

        public IEnumerable<GenreReadListDTO> Genres { get; set; }

        public IEnumerable<PlatformTypeReadListDTO> PlatformTypes { get; set; }
    }
}
