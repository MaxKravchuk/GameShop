using System.Linq;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class PriceFilter : IOperation<IQueryable<Game>>
    {
        private int _priceFrom;
        private int _priceTo;

        public IOperation<IQueryable<Game>> SetFilterData(int priceFrom, int priceTo)
        {
            _priceFrom = priceFrom;
            _priceTo = priceTo;
            return this;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IQueryable<Game> ApplyFilter(IQueryable<Game> games)
        {
            if (_priceFrom != 0)
            {
                games = games.Where(game => game.Price >= _priceFrom);
            }

            if (_priceTo != 0)
            {
                games = games.Where(game => game.Price <= _priceTo);
            }

            return games;
        }
    }
}
