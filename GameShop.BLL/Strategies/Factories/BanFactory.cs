using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Enums;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Strategies.BanStrategies;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using Unity;

namespace GameShop.BLL.Strategies.Factories
{
    public class BanFactory : IBanFactory
    {
        private readonly IUnityContainer _container;

        public BanFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IBanStrategy GetBanStrategy(BanOptions banOptions)
        {
            switch (banOptions)
            {
                case BanOptions.Hour:
                    return _container.Resolve<HourBanStrategy>();
                case BanOptions.Day:
                    return _container.Resolve<DayBanStrategy>();
                case BanOptions.Month:
                    return _container.Resolve<MonthBanStrategy>();
                case BanOptions.Week:
                    return _container.Resolve<WeekBanStrategy>();
                case BanOptions.Permanent:
                    return _container.Resolve<PermanentBanStrategy>();
                default:
                    throw new BadRequestException("Invalid ban option");
            }
        }
    }
}
