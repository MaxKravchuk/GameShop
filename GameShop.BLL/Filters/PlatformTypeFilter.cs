using System;
using System.Collections.Generic;
using System.Linq;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class PlatformTypeFilter : IOperation<IQueryable<Game>>
    {
        private IEnumerable<int> _platformTypesIds;

        public IOperation<IQueryable<Game>> SetFilterData(IEnumerable<int> platformTypesIds)
        {
            _platformTypesIds = platformTypesIds;
            return this;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IQueryable<Game> ApplyFilter(IQueryable<Game> games)
        {
            if (_platformTypesIds != null && _platformTypesIds.Any())
            {
                games = games.Where(game => game.GamePlatformTypes.Any(genre => _platformTypesIds.Contains(genre.Id)));
            }

            return games;
        }
    }
}
