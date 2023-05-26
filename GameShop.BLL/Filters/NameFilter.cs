using System.Linq;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class NameFilter : IOperation<IQueryable<Game>>
    {
        private string _gameName;

        public IOperation<IQueryable<Game>> SetFilterData(string gameName)
        {
            _gameName = gameName;
            return this;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IQueryable<Game> ApplyFilter(IQueryable<Game> games)
        {
            if (!string.IsNullOrEmpty(_gameName))
            {
                games = games.Where(game => game.Name.ToUpper().Contains(_gameName.ToUpper().Trim()));
            }

            return games;
        }
    }
}
