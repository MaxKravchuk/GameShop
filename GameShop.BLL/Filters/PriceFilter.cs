using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class PriceFilter : IOperation<IEnumerable<Game>>
    {
        private int _priceFrom;
        private int _priceTo;

        public IOperation<IEnumerable<Game>> SetFilterData(int priceFrom, int priceTo)
        {
            _priceFrom = priceFrom;
            _priceTo = priceTo;
            return this;
        }

        public IEnumerable<Game> Execute(IEnumerable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IEnumerable<Game> ApplyFilter(IEnumerable<Game> games)
        {
            if (_priceFrom != 0 && _priceTo != 0)
            {
                games = games.Where(game => game.Price > _priceFrom && game.Price < _priceTo);
            }

            return games;
        }
    }
}
