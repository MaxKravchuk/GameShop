using System.Linq;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.SortingStrategies
{
    public class DateStrategy : IGamesSortingStrategy
    {
        public IQueryable<Game> Sort(IQueryable<Game> games)
        {
            return games.OrderByDescending(game => game.CreatedAt);
        }
    }
}
