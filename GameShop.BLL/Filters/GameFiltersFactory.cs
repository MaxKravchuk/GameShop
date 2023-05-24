using System.Collections.Generic;
using System.Linq;
using GameShop.BLL.DTO.FilterDTOs;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;
using Unity;

namespace GameShop.BLL.Filters
{
    public class GameFiltersFactory : IFiltersFactory<IQueryable<Game>>
    {
        private readonly IUnityContainer _container;

        public GameFiltersFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IEnumerable<IOperation<IQueryable<Game>>> GetOperation(BaseFilterDTO baseFilterDTO)
        {
            var gameFilterDTO = baseFilterDTO as GameFiltersDTO;

            var operations = new List<IOperation<IQueryable<Game>>>
            {
                _container.Resolve<CreatedAtFilter>().SetFilterDate(gameFilterDTO.DateOption),
                _container.Resolve<GenreFilter>().SetFilterData(gameFilterDTO.GenreIds),
                _container.Resolve<NameFilter>().SetFilterData(gameFilterDTO.GameName),
                _container.Resolve<PlatformTypeFilter>().SetFilterData(gameFilterDTO.PlatformTypeIds),
                _container.Resolve<PriceFilter>().SetFilterData(gameFilterDTO.PriceFrom, gameFilterDTO.PriceTo),
                _container.Resolve<PublisherFilter>().SetFilterData(gameFilterDTO.PublisherIds)
            };

            return operations;
        }
    }
}
