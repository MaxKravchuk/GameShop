using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class PublisherFilter : IOperation<IEnumerable<Game>>
    {
        private IEnumerable<int> _publishersIds;

        public IOperation<IEnumerable<Game>> SetFilterData(IEnumerable<int> publishersIds)
        {
            _publishersIds = publishersIds;
            return this;
        }

        public IEnumerable<Game> Execute(IEnumerable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IEnumerable<Game> ApplyFilter(IEnumerable<Game> games)
        {
            if (_publishersIds != null && _publishersIds.Any())
            {
                games = games.Where(game => _publishersIds.Contains(game.Publisher.Id));
            }

            return games;
        }
    }
}
