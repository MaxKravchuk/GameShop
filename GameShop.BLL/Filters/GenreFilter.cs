using System;
using System.Collections.Generic;
using System.Linq;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class GenreFilter : IOperation<IQueryable<Game>>
    {
        private IEnumerable<int> _genreIds;

        public IOperation<IQueryable<Game>> SetFilterData(IEnumerable<int> genreIds)
        {
            _genreIds = genreIds;
            return this;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IQueryable<Game> ApplyFilter(IQueryable<Game> games)
        {
            if (_genreIds != null && _genreIds.Any())
            {
                games = games.Where(game => game.GameGenres.Any(genre => _genreIds.Contains(genre.Id)));
            }

            return games;
        }
    }
}
