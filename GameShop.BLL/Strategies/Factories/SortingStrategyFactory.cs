using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Strategies.Interfaces;
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

        public IGamesSortingStrategy GetGamesSortingStrategy(string sortingOption)
        {
            switch (sortingOption)
            {
                case "AscPrice":
                    return _container.Resolve<AscPriceStrategy>();
                case "Date":
                    return _container.Resolve<DateStrategy>();
                case "DescPrice":
                    return _container.Resolve<DescPriceStrategy>();
                case "MostCommented":
                    return _container.Resolve<MostCommentedStrategy>();
                case "MostPopular":
                    return _container.Resolve<MostPopularStrategy>();
                default:
                    throw new BadRequestException("Invalid sorting option");
            }
        }
    }
}
