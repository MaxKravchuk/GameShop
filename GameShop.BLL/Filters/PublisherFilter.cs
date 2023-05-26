using System;
using System.Collections.Generic;
using System.Linq;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class PublisherFilter : IOperation<IQueryable<Game>>
    {
        private IEnumerable<int> _publishersIds;

        public IOperation<IQueryable<Game>> SetFilterData(IEnumerable<int> publishersIds)
        {
            _publishersIds = publishersIds;
            return this;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IQueryable<Game> ApplyFilter(IQueryable<Game> games)
        {
            if (_publishersIds != null && _publishersIds.Any())
            {
                games = games.Where(game => _publishersIds.Contains(game.Publisher.Id));
            }

            return games;
        }
    }
}
