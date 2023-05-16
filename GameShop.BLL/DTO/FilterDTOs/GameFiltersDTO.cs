using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.FilterDTOs
{
    public class GameFiltersDTO : BaseFilterDTO
    {
        public IEnumerable<int> GenreIds { get; set; } = new List<int>();

        public IEnumerable<int> PlatformTypeIds { get; set; } = new List<int>();

        public IEnumerable<int> PublisherIds { get; set; } = new List<int>();

        public string DateOption { get; set; }

        public string GameName { get; set; }

        public int PriceFrom { get; set; }

        public int PriceTo { get; set; }
    }
}
