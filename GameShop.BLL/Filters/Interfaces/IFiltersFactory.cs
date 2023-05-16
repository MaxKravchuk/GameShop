using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.FilterDTOs;

namespace GameShop.BLL.Filters.Interfaces
{
    public interface IFiltersFactory<T>
    {
        IEnumerable<IOperation<T>> GetOperation(BaseFilterDTO baseFilterDTO);
    }
}
