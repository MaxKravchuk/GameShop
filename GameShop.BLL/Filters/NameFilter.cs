using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class NameFilter : IOperation<IEnumerable<Game>>
    {
        private string _gameName;

        public IOperation<IEnumerable<Game>> SetFilterData(string gameName)
        {
            _gameName = gameName;
            return this;
        }

        public IEnumerable<Game> Execute(IEnumerable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IEnumerable<Game> ApplyFilter(IEnumerable<Game> games)
        {
            if (!string.IsNullOrEmpty(_gameName))
            {
                games = games.Where(game => game.Name.ToUpper().Contains(_gameName.ToUpper().Trim()));
            }

            return games;
        }
    }
}
