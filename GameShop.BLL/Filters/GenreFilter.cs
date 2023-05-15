using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class GenreFilter : GameFilter
    {
        private readonly IEnumerable<int> _genreIds;

        public GenreFilter(IEnumerable<int> genreIds)
        {
            _genreIds = genreIds;
        }

        public override IEnumerable<Game> ApplyFilter(IEnumerable<Game> games)
        {
            if (_genreIds != null && _genreIds.Any())
            {
                games = games.Where(game => game.GameGenres.Any(genre => _genreIds.Contains(genre.Id)));
            }

            return games;
        }
    }
}
