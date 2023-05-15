using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.GameDTOs
{
    public class GameFiltersDTO
    {
        public IEnumerable<int> GenreIds { get; set; } = new List<int>();
    }
}
