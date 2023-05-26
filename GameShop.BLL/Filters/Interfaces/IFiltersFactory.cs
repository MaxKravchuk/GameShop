using System.Collections.Generic;
using GameShop.BLL.DTO.FilterDTOs;

namespace GameShop.BLL.Filters.Interfaces
{
    public interface IFiltersFactory<T>
    {
        IEnumerable<IOperation<T>> GetOperation(BaseFilterDTO baseFilterDTO);
    }
}
