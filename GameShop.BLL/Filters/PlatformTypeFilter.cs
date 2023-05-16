using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class PlatformTypeFilter : IOperation<IEnumerable<Game>>
    {
        private IEnumerable<int> _platformTypesIds;

        public IOperation<IEnumerable<Game>> SetFilterData(IEnumerable<int> platformTypesIds)
        {
            _platformTypesIds = platformTypesIds;
            return this;
        }

        public IEnumerable<Game> Execute(IEnumerable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IEnumerable<Game> ApplyFilter(IEnumerable<Game> games)
        {
            if (_platformTypesIds != null && _platformTypesIds.Any())
            {
                games = games.Where(game => game.GamePlatformTypes.Any(genre => _platformTypesIds.Contains(genre.Id)));
            }

            return games;
        }
    }
}
