﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class GenreFilter : IOperation<IEnumerable<Game>>
    {
        private IEnumerable<int> _genreIds;

        public IOperation<IEnumerable<Game>> SetFilterData(IEnumerable<int> genreIds)
        {
            _genreIds = genreIds;
            return this;
        }

        public IEnumerable<Game> Execute(IEnumerable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IEnumerable<Game> ApplyFilter(IEnumerable<Game> games)
        {
            if (_genreIds != null && _genreIds.Any())
            {
                games = games.Where(game => game.GameGenres.Any(genre => _genreIds.Contains(genre.Id)));
            }

            return games;
        }
    }
}
