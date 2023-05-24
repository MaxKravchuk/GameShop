using System.Linq;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.Interfaces.Strategies
{
    public interface IGamesSortingStrategy
    {
        IQueryable<Game> Sort(IQueryable<Game> games);
    }
}
