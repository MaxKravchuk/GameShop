using System.Collections.Generic;

namespace GameShop.BLL.DTO.GenreDTOs
{
    public class GenreReadListDTO : GenreBaseDTO
    {
        public int Id { get; set; }

        public int? ParentGenreId { get; set; }
    }
}
