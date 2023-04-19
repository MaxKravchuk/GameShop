using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.GenreDTOs
{
    public class GenreReadDTO : GenreBaseDTO
    {
        public int Id { get; set; }

        public IEnumerable<string> SubGenresName { get; set; } = new List<string>();

        public IEnumerable<string> GameKeys { get; set; } = new List<string>();
    }
}
