using GameShop.BLL.Enums;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.BLL.Strategies.SortingStrategies;
using Unity;

namespace GameShop.BLL.Strategies.Factories
{
    public class SortingStrategyFactory : IGameSortingFactory
    {
        private readonly IUnityContainer _container;

        public SortingStrategyFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IGamesSortingStrategy GetGamesSortingStrategy(SortingTypes sortingType)
        {
            switch (sortingType)
            {
                case SortingTypes.AscPrice:
                    return _container.Resolve<AscPriceStrategy>();
                case SortingTypes.Date:
                    return _container.Resolve<DateStrategy>();
                case SortingTypes.DescPrice:
                    return _container.Resolve<DescPriceStrategy>();
                case SortingTypes.MostCommented:
                    return _container.Resolve<MostCommentedStrategy>();
                case SortingTypes.MostPopular:
                    return _container.Resolve<MostPopularStrategy>();
                default:
                    throw new BadRequestException("Invalid sorting option");
            }
        }
    }
}
