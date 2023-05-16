using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Strategies.Interfaces;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.SortingStrategies
{
    public class AscPriceStrategy : IGamesSortingStrategy
    {
        public IEnumerable<Game> Sort(IEnumerable<Game> games)
        {
            return games.OrderBy(game => game.Price);
        }
    }
}
