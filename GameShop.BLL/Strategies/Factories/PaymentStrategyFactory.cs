using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Strategies.Interfaces;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.BLL.Strategies.PaymentStrategies;
using Unity;

namespace GameShop.BLL.Strategies.Factories
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IUnityContainer _container;

        public PaymentStrategyFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IPaymentStrategy GetPaymentStrategy(string paymentType)
        {
            switch (paymentType)
            {
                case "Visa":
                    return _container.Resolve<VisaStrategy>();
                case "Bank":
                    return _container.Resolve<BankStrategy>();
                case "iBox":
                    return _container.Resolve<IBoxStrategy>();
                default:
                    throw new BadRequestException("Invalid payment type");
            }
        }
    }
}
