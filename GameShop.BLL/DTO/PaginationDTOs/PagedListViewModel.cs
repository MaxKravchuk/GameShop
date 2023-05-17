using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.PaginationDTOs
{
    public class PagedListViewModel<T>
    {
        public bool HasPrevious { get; set; }

        public bool HasNext { get; set; }

        public IEnumerable<T> Entities { get; set; }
    }
}
