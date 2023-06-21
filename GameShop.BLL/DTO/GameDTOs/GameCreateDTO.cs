using System.Collections.Generic;

namespace GameShop.BLL.DTO.GameDTOs
{
    public class GameCreateDTO : GameBaseDTO
    {
        public string Description { get; set; }

        public decimal? Price { get; set; }

        public short? UnitsInStock { get; set; }

        public bool? Discontinued { get; set; } = true;

        public int? PublisherId { get; set; }

        public ICollection<int> GenresId { get; set; } = new List<int>();

        public ICollection<int> PlatformTypeId { get; set; } = new List<int>();
    }
}
