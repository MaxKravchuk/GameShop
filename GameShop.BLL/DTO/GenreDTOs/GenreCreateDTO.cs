using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.GenreDTOs
{
    public class GenreCreateDTO : GenreBaseDTO
    {
        public string ParentName { get; set; } = null;
    }
}
