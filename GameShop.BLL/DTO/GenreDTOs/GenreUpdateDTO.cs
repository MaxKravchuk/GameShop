using System.Collections.Generic;

namespace GameShop.BLL.DTO.GenreDTOs
{
    public class GenreUpdateDTO : GenreBaseDTO
    {
        public int Id { get; set; }

        public string ParentName { get; set; }

        public IEnumerable<string> SubGenres { get; set; } = new List<string>();

        public IEnumerable<string> GameGenres { get; set; } = new List<string>();
    }
}
