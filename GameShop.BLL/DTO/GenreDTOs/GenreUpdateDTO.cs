using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
