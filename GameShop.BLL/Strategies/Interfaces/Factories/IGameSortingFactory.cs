using GameShop.BLL.Enums;
using GameShop.BLL.Strategies.Interfaces.Strategies;

namespace GameShop.BLL.Strategies.Interfaces.Factories
{
    public interface IGameSortingFactory
    {
        IGamesSortingStrategy GetGamesSortingStrategy(SortingTypes sortingType);
    }
}
